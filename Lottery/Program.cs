using System;

namespace Lottery
{
    public class Program
    {
        //add layers to the application 

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
                    new Game(6, 1, Games.Thunderball).Play();
                }
                else
                {
                    new Game(6, 2, Games.Lotto).Play();
                }
            }
            else if(mode == Modes.Stats)
            {
                Console.WriteLine("Which stat to list?");
                String game = Console.ReadLine();
                if (game == "thunderball")
                {
                   // new Game(5, 1, Games.Thunderball)
                }
                else
                {
                    //new Lotto().GetStats();
                }
            }
        }
    }
}
