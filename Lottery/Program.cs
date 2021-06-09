using System;

namespace Lottery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter mode: ");
            String modeString = Console.ReadLine();
            Modes mode = Modes.NA;
            switch(modeString)
            {
                case "play":
                    mode = Modes.Play;
                    break;
                case "stats":
                    mode = Modes.Stats;
                    break;
                default:
                    break;
            }

            if(mode == Modes.Play)
            {
                Console.WriteLine("Which game to play?");
                String game = Console.ReadLine();
                if(game == "thunderball")
                {
                    new Thunderball().Play();
                }
                else
                {
                    new Lotto().Play();
                }
            }
            else if(mode == Modes.Stats)
            {
                Console.WriteLine("Which stat to list?");
                String game = Console.ReadLine();
                if (game == "thunderball")
                {
                    new Thunderball().GetStats();
                }
                else
                {
                    new Lotto().GetStats();
                }
            }
        }
    }
}
