using System;
using System.Collections.Generic;

namespace Lottery
{
    public class PrizeTable
    {
        //  numbersMatched + bonus, int
        private Dictionary<Tuple<int, bool>, int> prizeTable;

        /// <summary>
        /// add this to a table if possible
        /// </summary>
        /// <param name="game"></param>
        public PrizeTable(Games game)
        {
            prizeTable = new();
            if(game == Games.Lotto)
            {
                //Lucky Dip
                //Tuple<int, bool> pair = new(2, false);
                //prizeTable.Add(pair, -1);

                Tuple<int, bool> pair2 = new(3, false);
                prizeTable.Add(pair2, 30);

                Tuple<int, bool> pair3 = new(4, false);
                prizeTable.Add(pair3, 140);

                Tuple<int, bool> pair4 = new(5, false);
                prizeTable.Add(pair4, 1750);

                Tuple<int, bool> pair4B = new(5, true);
                prizeTable.Add(pair4B, 1000000);

                Tuple<int, bool> pair5 = new(6, false);
                prizeTable.Add(pair5, 2000000);
            }
            else
            {
                Tuple<int, bool> pair0B = new(0, true);
                prizeTable.Add(pair0B, 3);

                Tuple<int, bool> pair1B = new(1, true);
                prizeTable.Add(pair1B, 5);

                Tuple<int, bool> pair2B = new(2, true);
                prizeTable.Add(pair2B, 10);

                Tuple<int, bool> pair3 = new(3, false);
                prizeTable.Add(pair3, 10);

                Tuple<int, bool> pair3B = new(3, true);
                prizeTable.Add(pair3B, 20);

                Tuple<int, bool> pair4 = new(4, false);
                prizeTable.Add(pair4, 100);

                Tuple<int, bool> pair4B = new(4, true);
                prizeTable.Add(pair4B, 250);

                Tuple<int, bool> pair5 = new(5, false);
                prizeTable.Add(pair5, 5000);

                Tuple<int, bool> pair5B = new(5, true);
                prizeTable.Add(pair5B, 500000);
            }
        }

        public int GetPrize(int[] ballsDrawn, Dictionary<int, int[]> userLines)
        {
            int totalMatch = 0;
            int totalWon = 0;
            int index = 1;

            Console.WriteLine("\nResults");

            //loop through each of the user lines
            foreach (var line in userLines)
            {
                bool bonusMatched = false;

                int[] userNumbers = line.Value;
                for(int i = 1; i < userNumbers.Length; i++)
                {
                    for(int j = i + 1; j < userNumbers.Length; j++)
                    {
                        if(userNumbers[j] == ballsDrawn[i])
                        {
                            totalMatch++;
                        }
                    }
                }

                if(ballsDrawn[0] == userNumbers[0])
                {
                    bonusMatched = true;
                }

                Console.WriteLine("Game {0}, Number Matched: {1}, Bonus Matched: {2}.", index, totalMatch, bonusMatched);

                Tuple<int, bool> key = new(totalMatch, bonusMatched);
                if(prizeTable.ContainsKey(key))
                {
                    int won = prizeTable[key];
                    totalWon += won;
                }
                totalMatch = 0;
                index++;
            }

            return totalWon;
        }
    }
}
