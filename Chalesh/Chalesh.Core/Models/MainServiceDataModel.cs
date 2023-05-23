using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Models
{
    public class MainServiceDataModelIn
    {
        public Guid Id { get; set; }
        public DateTime SystemTime { get; set; }
        public int NumberofConnectedClients { get; set; }
    }
    public class MainServiceDataModelOut
    {
        public bool IsEnabled { get; set; }
        public int ExpirationTime { get; set; } // Based on sec 
        public int NumberOfActiveClients { get; set; }
    }
}
