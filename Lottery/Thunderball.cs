using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class Thunderball : IGame
    {
        private int price;

        public Thunderball()
        {
            price = 1;
        }

        private void OrderArray(int [] array)
        {
            for(int i = 0;  i < array.Length; i++)
            {
                for(int j = 1 + i; j < array.Length; j++)
                {
                    if(array[i] > array[j])
                    {
                        int t = array[i];
                        array[i] = array[j];
                        array[j] = t;
                    }
                }
            }
        }

        public int[] GenerateDraw()
        {
            const int NUMBER_OF_BALLS = 6;  //5 main ones, 1 bonus
            int[] numbers = new int[NUMBER_OF_BALLS];
            Random random = new Random();

            //main balls
            for(int i = 0; i < NUMBER_OF_BALLS - 1; i++)
            {
                int number = random.Next(1, 40);
                numbers[i] = number;
            }

            OrderArray(numbers);

            //bonus
            int thunderball = random.Next(1, 14);
            numbers[NUMBER_OF_BALLS - 1] = thunderball;

            return numbers;
        }

        public void GetStats()
        {
            throw new NotImplementedException();
        }

        public void SelectMode()
        {
            Console.WriteLine("Thunderball\nEnter mode:");
            String mode = Console.ReadLine();
            if(mode == "play")
            {
                Console.WriteLine("How many lines, £1 per line, 5 lines maximiun");
                String line = Console.ReadLine();
                if(int.TryParse(line, out int numberOfLines))
                {
                    int[][] allUserNumbers = new int[numberOfLines][];
                    int toPay = numberOfLines * price;

                    for(int i = 0; i < numberOfLines; i++)
                    {
                        Console.WriteLine("Enter your numbers for gamme {0}: ", i);
                        String numbersString = Console.ReadLine();
                        String[] numbers = numbersString.Split(" ");
                        int[] userNumbers = new int[numbers.Length];
                        for(int j = 0; j < numbers.Length; j++)
                        {
                            if(int.TryParse(numbers[i], out int number))
                            {
                                userNumbers[j] = number;
                            }
                        }

                        OrderArray(userNumbers);
                        allUserNumbers[i] = userNumbers;
                    }

                    int [] drawnNumbers = GenerateDraw();

                    String balls = "";
                    for(int i = 0; i < drawnNumbers.Length; i++)
                    {
                        if(i  < drawnNumbers.Length - 1)
                        {
                            balls += drawnNumbers[i] + " ";
                        }
                        else
                        {
                            balls += "Bonus: " + drawnNumbers[i];
                        }
                    }

                    Console.WriteLine("Drawn numbers: " + balls);
                    CheckWinnings();
                }
            }
            else if(mode == "search")
            {

            }
            else
            {
                GetStats();
            }
        }

        public void CheckWinnings()
        {
            throw new NotImplementedException();
        }
    }
}
