using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class Game
    {
        private int cost;
        private Dictionary<int, int[]> userLines;
        private int price;  //price of 1 game
        private Games game;

        private readonly int NUMBER_OF_BALLS;

        public Game(int numberOfBallsToBeDrawn, int price, Games game) : this(game)
        {
            NUMBER_OF_BALLS = numberOfBallsToBeDrawn;
            this.price = price;
        }

        public Game(Games game)
        {
            this.game = game;
        }

        public void Play()
        {
            Console.WriteLine("How many games, £{0} per line.", price);
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
                    int[] userNumbers = new int[NUMBER_OF_BALLS]; 
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

                int[] drawnNumbers = GenerateDraw();

                String balls = "";
                for (int i = 1; i < drawnNumbers.Length; i++)
                {
                    balls += drawnNumbers[i] + " ";
                }
                balls += "Bonus: " + drawnNumbers[0];
                Console.WriteLine("Drawn numbers: " + balls);
                CheckWinnings(drawnNumbers, userLines);
            }
        }

        private int[] GenerateDraw()
        {
            int[] drawnNumbers = new int[NUMBER_OF_BALLS];
            Random random = new Random();
            int maxNumber = 0;
            if (game == Games.Thunderball)
            {
                maxNumber = 39;
            }
            else
            {
                maxNumber = 59;
            }

            //populate the numbers in a list with the number and if selected
            Dictionary<int, bool> numbers = new();
            for (int i = 0; i < maxNumber; i++)
            {
                numbers.Add(i + 1, false);
            }


            //for 5 times, select an element, check if seen is false, then add the number to an array
            for (int i = 0; i < NUMBER_OF_BALLS - 1; i++)
            {
                bool valid = false;
                while (!valid)
                {
                    int index = random.Next(1, maxNumber);
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
            if (game == Games.Thunderball)
            {
                //important: we only populate the first 5 elements of the array so there a 0 in the last index.
                //after OrderArray the 0 is moved first, meaning when we print out the numbers, we need to start at index 1
                int thunderball = random.Next(1, 14);
                drawnNumbers[0] = thunderball;
            }
            else
            {
                //lottery selects 1 bonus that does not feature
                bool selected = false;
                while (!selected)
                {
                    int index = random.Next(1, maxNumber);
                    bool randomBallBeenSelected = numbers[index];
                    if (!randomBallBeenSelected)
                    {
                        numbers[index] = true;
                        drawnNumbers[0] = index;
                        selected = true;
                    }
                }
            }

            return drawnNumbers;
        }

        private void OrderArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 1 + i; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                    {
                        int t = array[i];
                        array[i] = array[j];
                        array[j] = t;
                    }
                }
            }
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
                        if (draw.SelectedGame == game.ToString())
                        {
                            //since its £1 per game, just add the total cost
                            numberPlayed++;

                            totalSpent += draw.Cost;
                            totalWon += draw.PrizeMoney;

                            if (draw.PrizeMoney > highestWinning)
                            {
                                highestWinning = draw.PrizeMoney;
                            }
                        }
                    }

                    Console.WriteLine("-----------Stats for {0}-------------", game.ToString());
                    Console.WriteLine("Total Games Played: {0}", numberPlayed);
                    Console.WriteLine("Total Spent: £{0}.00", totalSpent);


                    //remember int / int results in int
                    float averageWinning = (float)totalWon / (float)numberPlayed;
                    Console.WriteLine("Average Won: £" + averageWinning);
                    Console.WriteLine("Highest Winning: £{0}.00", highestWinning);

                    Console.WriteLine("Total Profit: £{0}.00", totalWon - totalSpent);
                    Console.WriteLine("Total Won: £{0}.00", totalWon);
                }
                else
                {
                    Console.WriteLine("No records");
                }
            }
        }

        private void Save(int[] drawNumbers, Games game, int prizeMoney)
        {
            //DrawContext context = null;
            DrawContext context = new();
            if (game == Games.Thunderball)
            {
                //context = 
            }
            else
            {
                //context =
            }



            using (context)
            {
                //get the ID first
                int newID = 0;
                var draws = context.Draw.ToList();
                if (draws != null && draws.Count > 0)
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
                draw.SetValues(drawNumbers, userLines, game);

                context.Draw.Add(draw);
                context.SaveChanges();
            }
        }

        private void CheckWinnings(int[] ballsDrawn, Dictionary<int, int[]> userLines)
        {
            PrizeTable prizeTable = new(game);
            int prize = prizeTable.GetPrize(ballsDrawn, userLines);
            int maxAmountMatch = prizeTable.AmountMatched;
            bool bonusMatched = prizeTable.BonusMatched;

            Console.WriteLine("Total cost: £{0}", cost);
            Console.WriteLine("Numbers matched: {0}, bonus matched: {1}", maxAmountMatch, bonusMatched);
            Console.WriteLine("You won: £{0}", prize);

            Save(ballsDrawn, game, prize);
        }
    }
}
