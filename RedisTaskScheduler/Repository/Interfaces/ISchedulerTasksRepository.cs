using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedisTaskScheduler.Entities;

namespace RedisTaskScheduler.Repository.Interfaces
{
    public interface ISchedulerTaskRepository<T>
    {        
        
        IAsyncEnumerable<T?>  GetSchedulerTasks();

        Task QueueSchedulerTask(T task);

        Task<T?> PopSchedulerTask();
        Task PushToRunning(T task, Guid id);
        
        Task PopFromRunning(Guid id);
        
        Task PushToDone(T task);
        
        void ClearRedis();
    }
}