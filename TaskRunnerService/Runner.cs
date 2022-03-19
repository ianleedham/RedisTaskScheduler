using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Entities.Tasks;
using RedisTaskScheduler.Repository.Interfaces;

namespace TaskRunnerService
{
    public class Runner<T> where T : SchedulerTask
    {
        private readonly ISchedulerTaskRepository<T> _taskRepository;
        private Guid Id { get; }
        public Runner(ISchedulerTaskRepository<T> taskRepository)
        {
            _taskRepository = taskRepository;
            Id = Guid.NewGuid();
        }
        public async Task Run()
        {
            T task = await _taskRepository.PopSchedulerTask();
            if (task is null) return;
            if (!await ReadyToRun(task)) return;

            await _taskRepository.PushToRunning(task, Id);
            var stopwatch = Stopwatch.StartNew();
            
            task.Run(Id);
            task.TimeTaken = stopwatch.Elapsed;
            await _taskRepository.PopFromRunning(Id);
            task.Status = ScheduledTaskStatus.Done;
            if(task.RepeatPeriod != null)
                task.NextRunTime = DateTime.Now.Add((TimeSpan)task.RepeatPeriod);
            await _taskRepository.PushToDone(task);

            await Requeue(task);
        }
        
        private async Task<bool> ReadyToRun(T task)
        {
            bool readyToRun = task.RepeatPeriod == null || task.NextRunTime < DateTime.Now;
            if(!readyToRun)                         
                await _taskRepository.QueueSchedulerTask(task); // requeue
            return readyToRun;
        }

        private async Task Requeue(T task)
        {            
            if (task.RepeatPeriod is null) return;
            task.RunTime = DateTime.MinValue;
            task.NextRunTime = DateTime.Now.Add((TimeSpan)task.RepeatPeriod); 
            task.Status = ScheduledTaskStatus.Queued;             
            await _taskRepository.QueueSchedulerTask(task); // requeue 
        }
    }
}