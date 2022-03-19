using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Entities.Tasks;
using StackExchange.Redis;

namespace RedisTaskScheduler.Repository
{
    public class TestSchedulerTasksRepository : SchedulerTaskRepository<TestSchedulerTask>
    {
        public TestSchedulerTasksRepository(IDatabase database) : base(database, "testKey")
        {
        }
    }
}