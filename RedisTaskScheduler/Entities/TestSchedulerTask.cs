using System;
using System.Threading;

namespace RedisTaskScheduler.Entities
{
    public class TestSchedulerTask : SchedulerTask
    {
        public string Message { get; set;  }
        public Guid RunnerId { get; set; }

        public TestSchedulerTask()
        {
        }
        
        public TestSchedulerTask(string message)
        {
            Id = Guid.NewGuid();
            Message = message;
        }

        public override void Run()
        {
            Thread.Sleep(5000);
            Console.WriteLine(Message);
        }
    }
}