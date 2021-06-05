using System;
using System.Collections.Generic;

namespace Lottery
{
    public class Thunderball : IGame
    {
        private int price;
        private IDictionary<int, int> prizeTable;
        private int drawNumber;

        public Thunderball(int drawNumber)
        {
            price = 1;
            this.drawNumber = drawNumber;
            prizeTable = new Dictionary<int, int>();
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
            for(int i = 0; i < NUMBER_OF_BALLS; i++)
            {
                int number = random.Next(1, 40);
                numbers[i] = number;
            }

            //check that the numbers do not have duplicates, if repeats then redraw
            //i < numbers.Length should be 5 because we are checking the selected balls
            bool foundDupliacte = false;
            for(int i = 0; i < numbers.Length; i++)
            {
                for(int j = i + 1; j < numbers.Length; j++)
                {
                    if(j != i && numbers[i] == numbers[j])
                    {
                        foundDupliacte = true;
                        break;
                    }
                }
            }

            if(foundDupliacte)
            {
                GenerateDraw();
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
                Dictionary<int, int[]> userLines = new Dictionary<int, int[]>();
                if (int.TryParse(line, out int numberOfLines))
                {
                    int toPay = numberOfLines * price;

                    Console.WriteLine("number of games: " + numberOfLines);
                    for(int i = 0; i < numberOfLines; i++)
                    {
                        int[] userNumbers = new int[6]; //5 main + 1 Bonus
                        Console.WriteLine("For Game {0}, do you want a lucky dip?", i + 1);
                        String asnwser = Console.ReadLine();
                        if(asnwser == "yes")
                        {
                            //this does not loop correctly if answer is yes
                            userNumbers = GenerateDraw();
                            String randomBalls = "";
                            for (int j = 0; j < userNumbers.Length; j++)
                            {
                                if (j < userNumbers.Length - 1)
                                {
                                    randomBalls += userNumbers[j] + " ";
                                }
                                else
                                {
                                    randomBalls += "Bonus: " + userNumbers[j];
                                }
                            }

                            Console.WriteLine("Your numbers: " + randomBalls);
                        }
                        else
                        {
                            Console.WriteLine("Enter your numbers for gamme {0}: ", i + 1);
                            String numbersString = Console.ReadLine();
                            String[] numbers = numbersString.Split(" ");
                            for (int j = 0; j < numbers.Length; j++)
                            {
                                if (int.TryParse(numbers[i], out int number))
                                {
                                    userNumbers[j] = number;
                                }
                            }

                            OrderArray(userNumbers);

                        }

                        userLines.Add(i, userNumbers);
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
                    CheckWinnings(drawnNumbers, userLines, toPay);
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

        public void CheckWinnings(int [] ballsDrawn, Dictionary<int, int[]> userLines, int totalCost)
        {
            int moneyWon = 0;
            int totalMatch = 0;
            bool bonusMatched = false;

            //loop through each of the user lines
            foreach (var line in userLines)
            {
                int[] userNumbers = line.Value;
                for(int i = 0;  i < userNumbers.Length; i++)
                {
                    for(int j = i + 1; j < userNumbers.Length; j++)
                    {
                        if(userNumbers[j] == ballsDrawn[i])
                        {
                            totalMatch++;
                        }
                    }

                    //compare the bonuses
                    if (ballsDrawn[ballsDrawn.Length - 1] == userNumbers[userNumbers.Length - 1])
                    {
                        bonusMatched = true;
                    }
                }
            }

            Console.WriteLine("Numbers matched: {0}, bonus matched: {1}", totalMatch, bonusMatched);

            Console.WriteLine("You won: £{0}", moneyWon);
            Console.WriteLine("Draw Number: {0}", drawNumber);
        }
    }
}
