using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Entities.Tasks;
using RedisTaskScheduler.Repository;
using StackExchange.Redis;

namespace TaskRunnerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDatabase _redis;

        public Worker(ILogger<Worker> logger, IDatabase redis)
        {
            _logger = logger;
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var testRunner1 = new Runner<TestSchedulerTask>(new TestSchedulerTasksRepository(_redis));
            var testRunner2 = new Runner<TestSchedulerTask>(new TestSchedulerTasksRepository(_redis));
            var urlRunner1 = new Runner<UrlSchedulerTask>(new UrlTaskRepository(_redis));
            var urlRunner2 = new Runner<UrlSchedulerTask>(new UrlTaskRepository(_redis));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                await testRunner1.Run();
                await testRunner2.Run();
                await urlRunner1.Run();
                await urlRunner2.Run();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}