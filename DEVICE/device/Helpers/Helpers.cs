using device.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace device
{
    public static class Helpers
    {
        public static Register GenerateData()
        {
            //Create Random Data
            Random rand = new Random();
            float d = rand.NextSingle();
            d += rand.Next(60);

            float d2 = rand.NextSingle();
            d2 += rand.Next(60);

            //Create Object Data
            Register reg = new Register() { temp = d, light = d2, timestamp = DateTime.Now };

            return reg;
        }

    }
}
