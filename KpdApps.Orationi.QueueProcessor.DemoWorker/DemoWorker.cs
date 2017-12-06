using KpdApps.Orationi.QueueProcessor.SDK;
using System;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading;

namespace KpdApps.Orationi.QueueProcessor.DemoWorker
{
    public class DemoWorker : WorkerBase
    {
        public override void Connect(string hostname)
        {
            base.Connect(hostname);
        }

        protected override void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            int dots = message.Split('.').Length - 1;
            Thread.Sleep(dots * 1000);

            Console.WriteLine(" [x] Done");

            IsFinished = true;
        }

        public override void BasicConsume()
        {
            base.BasicConsume();
        }
    }
}
