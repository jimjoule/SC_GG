﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace device.Models
{
    public class Register
    {
        public float temp { get; set; }
        public float light { get; set; }
        public DateTime timestamp { get; set; }
        public string deviceId { get; set; }

    }
}
