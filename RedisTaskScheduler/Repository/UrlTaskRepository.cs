using RedisTaskScheduler.Entities.Tasks;
using StackExchange.Redis;

namespace RedisTaskScheduler.Repository
{
    public class UrlTaskRepository : SchedulerTaskRepository<UrlSchedulerTask>
    {
        public UrlTaskRepository(IDatabase database) : base(database, "urlKey")
        {
        }
    }
}