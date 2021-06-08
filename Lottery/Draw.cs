using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lottery
{
    public class Draw
    {
        [Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string DrawnNumbers { get; set; }

        public string UserNumbers { get; set; }

        public int Cost { get; set; }

        public int PrizeMoney { get; set; }

        public string SelectedGame { get; set; }

        public void SetValues(int [] drawNumbers, Dictionary<int, int[]> UserLines, Games game)
        {
            //database saved drawn number as string
            foreach (int number in drawNumbers)
            {
                DrawnNumbers += number + " ";
            }

            //All user numbers
            foreach(var line in UserLines)
            {
                int[] numbers = line.Value;
                foreach(int number in numbers)
                {
                    UserNumbers += number + " ";
                }
                UserNumbers += "|";
            }

            //game is stored as string
            if(game == Games.Thunderball)
            {
                this.SelectedGame = Games.Thunderball.ToString();
            }
            else
            {
                this.SelectedGame = Games.Lotto.ToString();
            }
        }     
    }
}
