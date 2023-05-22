using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Models
{
    public class ConsumerModel
    {
        public int Id { get; set; }
        public string? Sender { get; set; }
        public string? Message { get; set; }
    }
}
