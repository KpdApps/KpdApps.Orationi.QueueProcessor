using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KpdApps.Orationi.QueueProcessor.DemoWorker;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace KpdApps.Orationi.QueueProcessor.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<DemoWorker.DemoWorker> workers = new List<DemoWorker.DemoWorker>();
            DemoWorker.DemoWorker worker = new DemoWorker.DemoWorker();
            worker.QueueName = "task_queue";
            worker.Connect("localhost");
            worker.BasicConsume();
            workers.Add(worker);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                CancellationTokenSource _shutdown = new CancellationTokenSource();
                AutoResetEvent _abortConnections = new AutoResetEvent(false);
                var token = _shutdown.Token;
                while (!token.IsCancellationRequested)
                {
                    uint count = channel.MessageCount("task_queue"); //.QueueDeclarePassive("task_queue");

                    if (count > 0)
                    {
                        for (int i = workers.Count; i <= 20; i++)
                        {
                            Func<string, bool> add = (hostname) =>
                            {
                                DemoWorker.DemoWorker demoWorker = new DemoWorker.DemoWorker();
                                demoWorker.QueueName = "task_queue";
                                demoWorker.Connect(hostname);
                                demoWorker.BasicConsume();
                                workers.Add(demoWorker);
                                return true;
                            };
                            add("localhost");
                        }
                    }

                    workers.RemoveAll(w => w.IsFinished);

                    Console.Clear();
                    Console.WriteLine("Avaliable workers: {0}\r\nIn Queue: {1}", workers.Count, count);

                    _abortConnections.WaitOne(100);
                }
            }
        }
    }
}
