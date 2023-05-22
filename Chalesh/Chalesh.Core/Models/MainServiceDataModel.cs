using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Models
{
    public class MainServiceDataModel
    {
        public Guid Id { get; set; }
        public DateTime SystemTime { get; set; }
        public int NumberofConnectedClients { get; set; }
    }
}
