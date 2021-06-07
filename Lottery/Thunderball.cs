using System;
using System.Linq;
using System.Collections.Generic;

namespace Lottery
{
    public class Thunderball : Game, IGame
    {
        private int price;  //price of 1 game

        private int cost;   //total price including multiple rows
        private Dictionary<int, int[]> userLines;
        private int prizeMoney;

        public Thunderball()
        {
            price = 1;
        }

        public int[] GenerateDraw()
        {
            const int NUMBER_OF_BALLS = 6;  //5 main ones, 1 bonus
            int[] drawnNumbers = new int[NUMBER_OF_BALLS];
            Random random = new Random();

            //populate the numbers in a list with the number and if selected
            Dictionary<int, bool> numbers = new();
            for(int i = 0; i < 39; i++)
            {
                numbers.Add(i + 1, false);
            }

            //for 5 times, select an element, check if seen is false, then add the number to an array
            for(int i = 0; i < 5; i++)
            {
                bool valid = false;
                while(!valid)
                {
                    int index = random.Next(1, 39);
                    bool selected = numbers[index];
                    if (!selected)
                    {
                        numbers[index] = true;
                        drawnNumbers[i] = index;
                        valid = true;
                    }
                }
              
            }

            OrderArray(drawnNumbers);

            //bonus ball
            //important: we only populate the first 5 elements of the array so there a 0 in the last index.
            //after OrderArray the 0 is moved first, meaning when we print out the numbers, we need to start at index 1
            int thunderball = random.Next(1, 14);
            drawnNumbers[0] = thunderball;

            return drawnNumbers;
        }

        public void GetStats()
        {
            int totalSpent = 0;
            int totalWon = 0;
            int numberPlayed = 0;
            int highestWinning = 0;
            using(DrawContext context = new())
            {
                List<Draw> draws = context.Draw.ToList();
                if(draws != null)
                {
                    foreach (Draw draw in draws)
                    {
                        if (draw.SelectedGame == "Thunderball")
                        {
                            //since its £1 per game, just add the total cost
                            numberPlayed += draw.Cost;

                            totalSpent += draw.Cost;
                            totalWon += draw.PrizeMoney;

                            if (draw.PrizeMoney > highestWinning)
                            {
                                highestWinning = draw.PrizeMoney;
                            }
                        }
                    }

                    Console.WriteLine("-----------Stats for Thunderball-------------");
                    Console.WriteLine("Total Games Played: {0}", numberPlayed);
                    Console.WriteLine("Total Spent: £{0}.00", totalSpent);

                    //remember int / int results in int
                    float averageWinning = (float)totalWon / (float)numberPlayed;
                    Console.WriteLine("Average Won: £" + averageWinning);
                    Console.WriteLine("Highest Winning: £{0}.00", highestWinning);
                    Console.WriteLine("Total Won: £{0}.00", totalWon);
                }
                else
                {
                    Console.WriteLine("No records");
                }
            }
        }

        public void Search()
        {
            Console.WriteLine("Enter Draw ID: ");
            String enteredID = Console.ReadLine();
            if(int.TryParse(enteredID, out int ID))
            {
                using (DrawContext context = new())
                {
                    if(context.Draw != null)
                    {
                        List<Draw> draws = context.Draw.ToList();
                        foreach (Draw draw in draws)
                        {
                            //really thunderball and lotto should be in its own table
                            if (draw.ID == ID && draw.SelectedGame == "Thunderball")
                            {
                                string drawnNumbers = draw.DrawnNumbers;

                               
                                Console.WriteLine("Drawn Numbers: " + drawnNumbers);
          
                                break;
                            }
                        }
                        
                    }
                }
            }
        }

        public void SelectMode()
        {
            Console.WriteLine("Thunderball\nEnter mode:");
            String mode = Console.ReadLine();
            if(mode == "play")
            {
                Console.WriteLine("How many games, £1 per line, 5 games maximiun");
                String line = Console.ReadLine();
                userLines = new Dictionary<int, int[]>();
                if (int.TryParse(line, out int numberOfLines))
                {
                    cost = numberOfLines * price;

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
                            for (int j = 1; j < userNumbers.Length; j++)    //see earlier why this starts at 1
                            {
                                randomBalls += userNumbers[j] + " ";
                            }

                            randomBalls += "Bonus: " + userNumbers[0];

                            Console.WriteLine("Your numbers: " + randomBalls);
                        }
                        else
                        {
                            Console.WriteLine("Enter your numbers for gamme {0}: ", i + 1);
                            Console.WriteLine("Select 5 mumbers between 1 and 39 with no repeats, and 1 bonus between 1 - 14");
                            String numbersString = Console.ReadLine();
                            String[] numbers = numbersString.Split(" ");
                            for (int j = 0; j < numbers.Length; j++) 
                            {
                                if (int.TryParse(numbers[i], out int number))
                                {
                                    userNumbers[j] = number;
                                    //error checking required
                                }
                            }

                            OrderArray(userNumbers);

                        }

                        userLines.Add(i, userNumbers);
                    }

                    int [] drawnNumbers = GenerateDraw();

                    String balls = "";
                    for(int i = 1; i < drawnNumbers.Length; i++)
                    {
                        balls += drawnNumbers[i] + " ";
                    }
                    balls += "Bonus: " + drawnNumbers[0];
                    Console.WriteLine("Drawn numbers: " + balls);
                    CheckWinnings(drawnNumbers, userLines, cost);
                }
            }
            else if(mode == "search")
            {
                Search();
            }
            else
            {
                GetStats();
            }
        }

        public void CheckWinnings(int [] ballsDrawn, Dictionary<int, int[]> userLines, int totalCost)
        {
            prizeMoney = 0;
            int totalMatch = 0;
            int maxAmountMatch = 0;
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
                    if (ballsDrawn[0] == userNumbers[0])
                    {
                        bonusMatched = true;
                    }

                    if(maxAmountMatch < totalMatch)
                    {
                        maxAmountMatch = totalMatch;
                    }

                    totalMatch = 0;
                }
            }

            if(maxAmountMatch == 0 && bonusMatched)
            {
                prizeMoney = 3;
            }
            else if(maxAmountMatch == 1 && bonusMatched)
            {
                prizeMoney = 5;
            }
            else if(maxAmountMatch == 2 && bonusMatched)
            {
                prizeMoney = 10;
            }
            else if(maxAmountMatch == 3)
            {
                prizeMoney = 10;
            }
            else if(maxAmountMatch == 3 && bonusMatched)
            {
                prizeMoney = 20;
            }
            else if(maxAmountMatch == 4)
            {
                prizeMoney = 100;
            }
            else if(maxAmountMatch == 4 && bonusMatched)
            {
                prizeMoney = 250;
            }
            else if(maxAmountMatch == 5)
            {
                prizeMoney = 5000;
            }
            else if(maxAmountMatch == 5 && bonusMatched)
            {
                prizeMoney = 500000;
            }
            else
            {
                prizeMoney = 0;
            }

            Console.WriteLine("Total cost: £{0}", cost);
            Console.WriteLine("Numbers matched: {0}, bonus matched: {1}", maxAmountMatch, bonusMatched);
            Console.WriteLine("You won: £{0}", prizeMoney);

            Save(ballsDrawn);
        }

        private void Save(int[] drawNumbers)
        {
            using(DrawContext context = new DrawContext())
            {
                //get the ID first
                int newID = 0;
                var draws = context.Draw.ToList();
                if(draws != null && draws.Count > 0)
                {
                    int lastID = draws.Last().ID;

                    //operator precedence, cant do newID = lastID++ since the variable
                    //is assigned first then incremented
                    newID = lastID;
                }
               
                newID++;

                Draw draw = new();
                draw.Cost = cost;
                draw.ID = newID;
                draw.PrizeMoney = prizeMoney;
                draw.SetValues(drawNumbers, userLines, Games.thunderball);

                context.Draw.Add(draw);
                context.SaveChanges();
            }
        }
    }
}
