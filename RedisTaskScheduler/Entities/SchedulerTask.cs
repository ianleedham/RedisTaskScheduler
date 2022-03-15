using System;
using System.Text.Json;

namespace RedisTaskScheduler.Entities
{
    public abstract class SchedulerTask
    {
        public Guid Id { get; set; }

        public TaskType Type { get; set; }

        public ScheduledTaskStatus Status { get; set; }

        public int Retries { get; set; }
        public int SecondsTaken { get; set; }

        public abstract void Run();

    }
}