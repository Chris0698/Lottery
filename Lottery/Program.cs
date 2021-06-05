using System;

namespace Lottery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Enter game: ");
                String mode = Console.ReadLine();
                if(mode == "lotto")
                {
                    
                }
                else if(mode == "thunderball")
                {
                    IGame thunderball = new Thunderball(1);
                    thunderball.SelectMode();
                }
            }
        }
    }
}
