﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Models
{
    public class ProducerModel
    {
        public int Id { get; set; }
        public int MessageLenght { get; set; }
        public string? Engine { get; set; }
        public bool IsValid { get; set; }

    }
}
