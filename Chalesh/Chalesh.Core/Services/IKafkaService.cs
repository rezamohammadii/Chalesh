using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Services
{
    public interface IKafkaService
    {
        void Listen(Action<string> message, string topic, CancellationToken cancellation);
        void Producer(string topic, string message);
    }
}
