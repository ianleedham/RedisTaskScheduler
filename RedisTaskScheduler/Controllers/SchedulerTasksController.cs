using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Repository.Interfaces;

namespace RedisTaskScheduler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchedulerTasksController : ControllerBase
    {
        private readonly ISchedulerTaskRepository _schedulerTaskRepository;

        public SchedulerTasksController(ISchedulerTaskRepository schedulerTaskRepository)
        {
            _schedulerTaskRepository = schedulerTaskRepository;
        }

        [HttpGet("TestEnqueue")]
        public void TestEnqueue()
        {
            var task = new TestSchedulerTask("Test message");
            _schedulerTaskRepository.UpdateSchedulerTask(task);
        }
        
        [HttpGet("GetRunnerHistory")]
        public IAsyncEnumerable<TestSchedulerTask?> GetRunnerHistory()
        {
            return _schedulerTaskRepository.GetSchedulerTasks();
        }
        
        [HttpGet("ClearRedis")]
        public void ClearRedis()
        {
            _schedulerTaskRepository.ClearRedis();
        }
    }
}