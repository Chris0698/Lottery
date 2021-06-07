using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lottery
{
    public class Draw
    {
        [Key]
        public int ID { get; set; }

        //public int[] DrawnNumbers { get; set; }

        public string Numbers { get; private set; }

        [NotMapped]
        public Dictionary<int, int[]> UserLines { get; set; }

        public int Cost { get; set; }

        public int PrizeMoney { get; set; }

        public Games Game { get; set; } //might not be needed

        public void SetNumbers(int[]numbers)
        {
            String line = "";
            foreach(int number in numbers)
            {
                line += number + " ";
            }
            Numbers = line;
        }
     
    }
}
