using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class Lotto : Game, IGame
    {
        public Lotto() : base(7, 2, Games.Lotto)
        {

        }

        public void CheckWinnings(int[] ballsDrawn, Dictionary<int, int[]> userLines, int totalCost, bool verbose = false)
        {
            prizeMoney = 0;
            int totalMatch = 0;
            int maxAmountMatch = 0;
            bool bonusMatched = false;

            //loop through each of the user lines
            foreach (var line in userLines)
            {
                int[] userNumbers = line.Value;
                for (int i = 0; i < userNumbers.Length; i++)
                {
                    for (int j = i + 1; j < userNumbers.Length; j++)
                    {
                        if (userNumbers[j] == ballsDrawn[i])
                        {
                            totalMatch++;
                        }
                    }

                    //compare the bonuses
                    if (ballsDrawn[0] == userNumbers[0])
                    {
                        bonusMatched = true;
                    }

                    if (maxAmountMatch < totalMatch)
                    {
                        maxAmountMatch = totalMatch;
                    }

                    totalMatch = 0;
                }
            }

            if (maxAmountMatch == 2)
            {
                //lucky dip
            }
            else if (maxAmountMatch == 3)
            {
                prizeMoney = 30;
            }
            else if (maxAmountMatch == 4)
            {
                prizeMoney = 140;
            }
            else if (maxAmountMatch == 5)
            {
                prizeMoney = 1750;
            }
            else if (maxAmountMatch == 5 && bonusMatched)
            {
                prizeMoney = 1000000;
            }
            else if (maxAmountMatch == 6)
            {
                prizeMoney = 2000000;
            }
            else
            {
                prizeMoney = 0;
            }

            Console.WriteLine("Total cost: £{0}", cost);
            Console.WriteLine("Numbers matched: {0}, bonus matched: {1}", maxAmountMatch, bonusMatched);
            Console.WriteLine("You won: £{0}", prizeMoney);

            Save(ballsDrawn, Games.Lotto);
        }

        public void GetStats()
        {
            int totalSpent = 0;
            int totalWon = 0;
            int numberPlayed = 0;
            int highestWinning = 0;
            using (DrawContext context = new())
            {
                List<Draw> draws = context.Draw.ToList();
                if (draws != null)
                {
                    foreach (Draw draw in draws)
                    {
                        if (draw.SelectedGame == "Lotto")
                        {
                            //since its £2 per game, just add the total cost / 2
                            //this doesn not work for lucky dips
                            numberPlayed += draw.Cost / 2;

                            totalSpent += draw.Cost;
                            totalWon += draw.PrizeMoney;

                            if (draw.PrizeMoney > highestWinning)
                            {
                                highestWinning = draw.PrizeMoney;
                            }
                        }
                    }

                    Console.WriteLine("-----------Stats for Lotto-------------");
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

        public void Play()
        {
            Console.WriteLine("How many games, £2 per line.");
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
                        Console.WriteLine("Select 6 mumbers between 1 and 59 with no repeats.");
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

        public void Search()
        {
            throw new NotImplementedException();
        }

        public void SelectMode()
        {
            Console.WriteLine("Lotto\nEnter mode:");
            String mode = Console.ReadLine();
            if (mode == "play")
            {
                Play();
            }
            else if (mode == "search")
            {
                Search();
            }
            else
            {
                GetStats();
            }
        }
    }
}
