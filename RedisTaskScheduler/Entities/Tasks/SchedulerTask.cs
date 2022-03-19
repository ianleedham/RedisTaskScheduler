using System;

namespace RedisTaskScheduler.Entities.Tasks
{
    public abstract class SchedulerTask
    {
        protected SchedulerTask()
        {
        }
        
        protected SchedulerTask(TimeSpan repeatPeriod)
        {
            RepeatPeriod = repeatPeriod;
            NextRunTime = DateTime.Now + repeatPeriod;
        }

        public Guid Id { get; set; }
        
        public Guid RunnerId { get; set; }

        public TaskType Type { get; set; }

        public ScheduledTaskStatus Status { get; set; }

        public int Retries { get; set; }
        
        public TimeSpan TimeTaken { get; set; }

        public TimeSpan? RepeatPeriod { get; set; }

        public DateTime RunTime { get; set; }
        
        public DateTime NextRunTime { get; set; }

        public abstract void Run(Guid runnerId);

        protected new string ToString()
        {
            return
                $" Status: {Status.ToString()} | Runner: {RunnerId} | Type: {Type.ToString()} | RunTime {RunTime} " +
                $"| SecondsTaken: {TimeTaken.Seconds} | MilliSecondsTaken: {TimeTaken.Milliseconds} " +
                $"| NextRunTime: {NextRunTime} ";
        }

    }
}