#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Entities.Tasks;
using RedisTaskScheduler.Repository.Interfaces;
using StackExchange.Redis;

namespace RedisTaskScheduler.Repository
{
    public class SchedulerTaskRepository<T>: ISchedulerTaskRepository<T> where T : SchedulerTask
    {
        private readonly IDatabase _redisCache;
        private readonly string _key;
        private string QueuedKey => $"{_key}-queued";
        private string RunningKey => $"{_key}-running";
        private string DoneKey => $"{_key}-done";

        protected SchedulerTaskRepository(IDatabase database, string key)
        {
            _redisCache = database ?? throw new ArgumentNullException(nameof(database));
            _key = key;
        }

        public async IAsyncEnumerable<T?> GetSchedulerTasks()
        {
            var queuedTasks = await _redisCache.ListRangeAsync(QueuedKey);
            var runningTasks = await _redisCache.ListRangeAsync(RunningKey);
            var doneTasks = await _redisCache.ListRangeAsync(DoneKey);

            if(queuedTasks.Length > 0)
                foreach (var taskString in queuedTasks)
                {
                    var task = JsonConvert.DeserializeObject<T>(taskString);
                    yield return task;
                }

            if(queuedTasks.Length > 0)
                foreach (var taskString in runningTasks)
                {
                    var task = JsonConvert.DeserializeObject<T>(taskString);
                    yield return task;
                }
            
            if(doneTasks.Length > 0)
                foreach (var taskString in doneTasks)
                {
                    var task = JsonConvert.DeserializeObject<T>(taskString);
                    yield return task;
                }
        }
        
        public async Task QueueSchedulerTask(T task)
        {
            var taskString = JsonConvert.SerializeObject(task);
            await _redisCache.ListLeftPushAsync(QueuedKey, taskString);
        }
        
        public async Task<T?> PopSchedulerTask()
        {
            var taskString = await _redisCache.ListRightPopAsync(QueuedKey);
            if (!taskString.HasValue)
                return null;
            return JsonConvert.DeserializeObject<T>(taskString);
        }

        public async Task PushToRunning(T task, Guid id)
        {
            task.Status = ScheduledTaskStatus.Running;
            task.RunnerId = id;
            var taskString = JsonConvert.SerializeObject(task);
            await _redisCache.ListLeftPushAsync(RunningKey, taskString);
        }
        
        public async Task PopFromRunning(Guid id)
        {
            await _redisCache.ListRightPopAsync(RunningKey);
        }
        
        public async Task PushToDone(T task)
        {
            var taskString = JsonConvert.SerializeObject(task);
            await _redisCache.ListLeftPushAsync(DoneKey, taskString);
        }
        
        public void ClearRedis()
        {
            while (_redisCache.ListLength(QueuedKey) != 0)
                _redisCache.ListRightPop(QueuedKey);
            while (_redisCache.ListLength(RunningKey) != 0)
                _redisCache.ListRightPop(RunningKey);
            while (_redisCache.ListLength(DoneKey) != 0)
                _redisCache.ListRightPop(DoneKey);
        }

    }
}