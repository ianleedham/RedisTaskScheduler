using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Repository.Interfaces;
using StackExchange.Redis;

namespace RedisTaskScheduler.Repository
{
    public class SchedulerTaskRepository: ISchedulerTaskRepository
    {
        private readonly IDatabase _redisCache;
        public SchedulerTaskRepository(IDatabase database)
        {
            _redisCache = database ?? throw new ArgumentNullException(nameof(database));
        }
        
        public async IAsyncEnumerable<TestSchedulerTask?> GetSchedulerTasks()
        {
            var queuedTasks =_redisCache.ListRange("queued");
            var doneTasks =_redisCache.ListRange("done");

            if(queuedTasks.Length > 0)
                foreach (var taskString in queuedTasks)
                {
                    var task = JsonConvert.DeserializeObject<TestSchedulerTask>(taskString);
                    task.Status = ScheduledTaskStatus.Queued;
                    yield return task;
                }
            if(doneTasks.Length > 0)
                foreach (var taskString in doneTasks)
                {
                    var task = JsonConvert.DeserializeObject<TestSchedulerTask>(taskString);
                    task.Status = ScheduledTaskStatus.Done;
                    yield return task;
                }
        }
        
        public async Task<TestSchedulerTask> PopSchedulerTask()
        {
            var taskString = await _redisCache.ListRightPopAsync("queued");
            if (string.IsNullOrEmpty(taskString))
                return null;
            return JsonConvert.DeserializeObject<TestSchedulerTask>(taskString);

        }
         
        public async void UpdateSchedulerTask(TestSchedulerTask task)
        {
            var taskString = JsonConvert.SerializeObject(task);
            await _redisCache.ListLeftPushAsync("queued", taskString);
        }

        public void ClearRedis()
        {
            while (_redisCache.ListLength("queued") != 0)
                _redisCache.ListRightPop("queued");
            while (_redisCache.ListLength("running") != 0)
                _redisCache.ListRightPop("running");
            while (_redisCache.ListLength("done") != 0)
                _redisCache.ListRightPop("done");
        }
    }
}