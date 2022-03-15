using System.Collections.Generic;
using System.Threading.Tasks;
using RedisTaskScheduler.Entities;

namespace RedisTaskScheduler.Repository.Interfaces
{
    public interface ISchedulerTaskRepository
    {        
        
        IAsyncEnumerable<TestSchedulerTask>  GetSchedulerTasks();

        Task<TestSchedulerTask> PopSchedulerTask();
        
        void UpdateSchedulerTask(TestSchedulerTask task);


        void ClearRedis();
    }
}