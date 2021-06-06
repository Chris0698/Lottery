using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class Draw
    {
        public int ID { get; set; }

        public int[] DrawnNumbers { get; set; }

        public Dictionary<int, int[]> UserLines { get; set; }

        public int Cost { get; set; }

        public int PrizeMoney { get; set; }
     
    }
}
