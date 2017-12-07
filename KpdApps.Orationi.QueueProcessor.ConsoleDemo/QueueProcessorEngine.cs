using System;
using KpdApps.Orationi.QueueProcessor.SDK.Classes;
using KpdApps.Orationi.QueueProcessor.SDK.Interfaces;

namespace KpdApps.Orationi.QueueProcessor.ConsoleDemo
{
    public class QueueProcessorEngine
    {
        WorkerPool<IWorker> workersPool;
        public QueueProcessorEngine(string hostname, string queuename)
        {
            Func<DemoWorker.DemoWorker> workerGenerator = () =>
                {
                    DemoWorker.DemoWorker worker = new DemoWorker.DemoWorker();
                    worker.QueueName = queuename;
                    worker.Connect(hostname);
                    worker.BasicConsume();

                    return worker;
                };

            workersPool = new WorkerPool<IWorker>(workerGenerator);
        }
    }
}
