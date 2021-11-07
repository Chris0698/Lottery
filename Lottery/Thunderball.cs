using System;
using System.Linq;
using System.Collections.Generic;

namespace Lottery
{
    public class Thunderball : Game, IGame
    {
        public Thunderball() : base(6, 1, Games.Thunderball)
        {
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

        public void Play()
        {
            Console.WriteLine("AI or manual?");
            string input = Console.ReadLine();
            if(input == "AI")
            {
                Console.WriteLine("How many games, £1 per line.");
                String line = Console.ReadLine();
                userLines = new Dictionary<int, int[]>();
                if (int.TryParse(line, out int numberOfLines))
                {
                    while (numberOfLines < 1)
                    {
                        Console.WriteLine("Can't have 0 or less lines, enter number of lines");
                        line = Console.ReadLine();
                        if (int.TryParse(line, out int numberOfLines2))
                        {
                            numberOfLines = numberOfLines2;
                        }
                    }

                    cost = numberOfLines * price;

                    Console.WriteLine("number of games: " + numberOfLines);
                    for (int i = 0; i < numberOfLines; i++)
                    {
                        userLines.Add(i, GenerateDraw());
                    }
                }

                int[] drawnNumbers = GenerateDraw();
                String balls = "";
                for (int i = 1; i < drawnNumbers.Length; i++)
                {
                    balls += drawnNumbers[i] + " ";
                }
                balls += "Bonus: " + drawnNumbers[0];
                Console.WriteLine("Drawn numbers: " + balls);
                CheckWinnings(drawnNumbers, userLines, cost, true);
            }
            else
            {
                Console.WriteLine("How many games, £1 per line.");
                String line = Console.ReadLine();
                userLines = new Dictionary<int, int[]>();
                if (int.TryParse(line, out int numberOfLines))
                {
                    while (numberOfLines < 1)
                    {
                        Console.WriteLine("Can't have 0 or less lines, enter number of lines");
                        line = Console.ReadLine();
                        if (int.TryParse(line, out int numberOfLines2))
                        {
                            numberOfLines = numberOfLines2;
                        }
                    }

                    cost = numberOfLines * price;

                    Console.WriteLine("number of games: " + numberOfLines);
                    for (int i = 0; i < numberOfLines; i++)
                    {
                        int[] userNumbers = new int[NUMBER_OF_BALLS]; //5 main + 1 Bonus
                        Console.WriteLine("For Game {0}, do you want a lucky dip?", i + 1);
                        String asnwser = Console.ReadLine();
                        if (asnwser == "yes")
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
                }

                int[] drawnNumbers = GenerateDraw();
                String balls = "";
                for (int i = 1; i < drawnNumbers.Length; i++)
                {
                    balls += drawnNumbers[i] + " ";
                }
                balls += "Bonus: " + drawnNumbers[0];
                Console.WriteLine("Drawn numbers: " + balls);
                CheckWinnings(drawnNumbers, userLines, cost);
            }


          
        }

        public void SelectMode()
        {
            Console.WriteLine("Thunderball\nEnter mode:");
            String mode = Console.ReadLine();
            if(mode == "play")
            {
                Play();
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

        public void CheckWinnings(int [] ballsDrawn, Dictionary<int, int[]> userLines, int totalCost, bool verbose = false)
        {
            prizeMoney = 0;
            int totalMatch = 0;
            int maxAmountMatch = 0;
            bool bonusMatched = false;
            bool wasBonusMatchedInAddition = false;

            if(verbose)
            {
                int totalWinngs = 0;
                prizeMoney = 0;

                int count = 0;
                foreach (var line in userLines)
                {
                    count++;
                    int[] userNumbers = line.Value;
                    prizeMoney = 0;
                    totalMatch = 0;
                    bonusMatched = false;

                    //compare the bonuses
                    if (ballsDrawn[0] == userNumbers[0])
                    {
                        bonusMatched = true;
                    }

                    //compare the main numbers
                    for (int i = 1; i < userNumbers.Length; i++)
                    {
                        for (int j = 0; j < userNumbers.Length; j++)
                        {
                            if (userNumbers[i] == ballsDrawn[j])
                            {
                                totalMatch++;
                            }
                        }
                    }

                    if (maxAmountMatch < totalMatch)
                    {
                        maxAmountMatch = totalMatch;
                        wasBonusMatchedInAddition = bonusMatched;
                    }

                    if (totalMatch == 0 && bonusMatched)
                    {
                        prizeMoney = 3;
                    }
                    else if (totalMatch == 1 && bonusMatched)
                    {
                        prizeMoney = 5;
                    }
                    else if (totalMatch == 2 && bonusMatched)
                    {
                        prizeMoney = 10;
                    }
                    else if (totalMatch == 3)
                    {
                        prizeMoney = 10;
                    }
                    else if (totalMatch == 3 && bonusMatched)
                    {
                        prizeMoney = 20;
                    }
                    else if (totalMatch == 4)
                    {
                        prizeMoney = 100;
                    }
                    else if (totalMatch == 4 && bonusMatched)
                    {
                        prizeMoney = 250;
                    }
                    else if (totalMatch == 5)
                    {
                        prizeMoney = 5000;
                    }
                    else if (totalMatch == 5 && bonusMatched)
                    {
                        prizeMoney = 500000;
                    }
                    else
                    {
                        prizeMoney = 0;
                    }

                    totalWinngs += prizeMoney;
                    Console.WriteLine("Game {2} numbers: {3}. Numbers matched: {0}, bonus matched: {1}. Winngs of: £{4}.", totalMatch, bonusMatched, count, userLines.Values, prizeMoney);
                }

                Console.WriteLine("Total cost: £{0}", cost);
                Console.WriteLine("You won: £{0}", totalWinngs);
                Console.WriteLine("Best result: {0} matches and bonus matched: {1}", maxAmountMatch, wasBonusMatchedInAddition);
                Console.WriteLine("Total profit: £" + (totalWinngs - cost));
                prizeMoney = totalWinngs;   //need this for the databvase
                Save(ballsDrawn, Games.Thunderball);
            }
            else
            {
                //loop through each of the user lines
                foreach (var line in userLines)
                {
                    int[] userNumbers = line.Value;
                    //compare the bonuses
                    if (ballsDrawn[0] == userNumbers[0])
                    {
                        bonusMatched = true;
                    }

                    //compare the main numbers
                    for (int i = 1; i < userNumbers.Length; i++)
                    {
                        for (int j = 0; j < userNumbers.Length; j++)
                        {
                            if (userNumbers[i] == ballsDrawn[j])
                            {
                                totalMatch++;
                            }
                        }
                    }

                    if (maxAmountMatch < totalMatch)
                    {
                        maxAmountMatch = totalMatch;
                    }

                    totalMatch = 0;
                }

                if (maxAmountMatch == 0 && bonusMatched)
                {
                    prizeMoney = 3;
                }
                else if (maxAmountMatch == 1 && bonusMatched)
                {
                    prizeMoney = 5;
                }
                else if (maxAmountMatch == 2 && bonusMatched)
                {
                    prizeMoney = 10;
                }
                else if (maxAmountMatch == 3)
                {
                    prizeMoney = 10;
                }
                else if (maxAmountMatch == 3 && bonusMatched)
                {
                    prizeMoney = 20;
                }
                else if (maxAmountMatch == 4)
                {
                    prizeMoney = 100;
                }
                else if (maxAmountMatch == 4 && bonusMatched)
                {
                    prizeMoney = 250;
                }
                else if (maxAmountMatch == 5)
                {
                    prizeMoney = 5000;
                }
                else if (maxAmountMatch == 5 && bonusMatched)
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

                Save(ballsDrawn, Games.Thunderball);
            }

           
        }
    }
}
