using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpdApps.Orationi.QueueProcessor.SDK.Interfaces
{
    public interface IWorker
    {
        void Connect(string hostname);

        void BasicConsume();
    }
}
