using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var runner1 = new Runner(_redis);
                runner1.Run();
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}