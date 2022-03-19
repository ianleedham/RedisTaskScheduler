using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RedisTaskScheduler.Entities.Tasks;
using RedisTaskScheduler.Repository.Interfaces;

namespace RedisTaskScheduler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchedulerTasksController : ControllerBase
    {
        private readonly ISchedulerTaskRepository<TestSchedulerTask> _testTaskRepository;
        private readonly ISchedulerTaskRepository<UrlSchedulerTask> _urlTaskRepository;

        public SchedulerTasksController(ISchedulerTaskRepository<TestSchedulerTask> testTaskRepository,
            ISchedulerTaskRepository<UrlSchedulerTask> urlTaskRepository)
        {
            _testTaskRepository = testTaskRepository;
            _urlTaskRepository = urlTaskRepository;
        }

        [HttpGet("TestEnqueue")]
        public void TestEnqueue()
        {
            var task = new TestSchedulerTask("Test message", TimeSpan.FromMinutes(2));
            _testTaskRepository.QueueSchedulerTask(task);
        }
        
        [HttpGet("TestEnqueue2")]
        public void TestEnqueue2()
        {
            var task = new UrlSchedulerTask();
            _urlTaskRepository.QueueSchedulerTask(task);
        }
        
        [HttpGet("GetQueue")]
        public async IAsyncEnumerable<string> GetQueue()
        {
            var testTasks = _testTaskRepository.GetSchedulerTasks();
            var urlTasks = _urlTaskRepository.GetSchedulerTasks();
            await foreach (var testTask in testTasks)
                yield return testTask?.ToString();
            await foreach (var testTask in urlTasks)
                yield return testTask?.ToString();
        }
        
        [HttpGet("ClearRedis")]
        public void ClearRedis()
        {
            _testTaskRepository.ClearRedis();
            _urlTaskRepository.ClearRedis();
        }
    }
}