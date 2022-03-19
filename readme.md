This solution is composed of two projects. 
The RedisTaskScheduler is for adding and viewing tasks in the redis queues.
The TaskRunnerService is for taking the tasks from the redis queue "queued" adding them to the running queue, running the task and adding it to the done queue.

There are currently two types of task:
The test task which simply waits the thread 5 seconds.
The url task which calls a url via a http client.

The tasks when created by the api can also be marked to be rerun every 2 minutes.


Todo:
add database interacting tasks.
create a live dashboard to show tasks statuses 
scale up the number of runners and tasks 
move done tasks out of redis?
decide what to do if a runner goes down while running a task. (Rerunning)
