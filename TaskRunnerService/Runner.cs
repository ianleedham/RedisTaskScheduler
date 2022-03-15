using System;
using System.Diagnostics;
using Newtonsoft.Json;
using RedisTaskScheduler.Entities;
using StackExchange.Redis;

namespace TaskRunnerService
{
    public class Runner
    {
        private readonly IDatabase _redis;
        public Guid Id { get; }
        public Runner(IDatabase redis)
        {
            _redis = redis;
            Id = Guid.NewGuid();
        }
        public void Run()
        {
            var taskString = _redis.ListRightPop("queued");
            if (!taskString.HasValue)
                return;
            var task = JsonConvert.DeserializeObject<TestSchedulerTask>(taskString);
            task.Status = ScheduledTaskStatus.Running;
            
            task.RunnerId = Id;
            
            taskString = JsonConvert.SerializeObject(task);
            _redis.ListLeftPush("Running", taskString);
            _redis.ListLeftPush($"{Id}-Running", taskString);
            
            var stopwatch = Stopwatch.StartNew();
            task?.Run();
            _redis.ListRightPop($"{Id}-Running");
            task.SecondsTaken = stopwatch.Elapsed.Seconds;
            task.Status = ScheduledTaskStatus.Done;
            taskString = JsonConvert.SerializeObject(task);
            _redis.ListLeftPush($"done", taskString);
            
        }
    }
}