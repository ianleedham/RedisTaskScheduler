using System;
using System.Net.Http;

namespace RedisTaskScheduler.Entities.Tasks
{
    public class UrlSchedulerTask: SchedulerTask
        {
            private readonly HttpClient _client;

            public string Url { get; }
            
            public UrlSchedulerTask(string url = "https://www.google.com")
            {
                _client = new HttpClient();
                Id = Guid.NewGuid();
                Url = url;
                Type = TaskType.Url;
            }

            public override void Run(Guid runnerId)
            {
                RunnerId = runnerId;
                var response = _client.GetAsync(Url).Result;
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }


            public new string ToString()
            {
                return $"Url: {Url} | {base.ToString()}";
            }
        }
    }
