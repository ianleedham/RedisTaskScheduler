using System;
using System.Threading;

namespace RedisTaskScheduler.Entities.Tasks
{
    public class TestSchedulerTask : SchedulerTask
    {
        public string Message { get; set;  }
        
        public TestSchedulerTask(string message, TimeSpan repeatPeriod) : base(repeatPeriod)
        {
            Id = Guid.NewGuid();
            Message = message;
            Type = TaskType.Test;
        }

        public override void Run(Guid runnerId)
        {
            RunTime = DateTime.Now;
            RunnerId = runnerId;
            Thread.Sleep(5000);
            Console.WriteLine(Message);
        }

        public new string ToString()
        {
            return $"Message: {Message} | {base.ToString()}";
        }
    }
}