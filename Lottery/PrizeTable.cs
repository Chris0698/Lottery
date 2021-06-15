using System.Collections.Generic;

namespace Lottery
{
    public class PrizeTable
    {
        //  numbersMatched + bonus, int
        private Dictionary<Dictionary<int, bool>, int> prizeTable;
        public int AmountMatched { get; private set; }
        public bool BonusMatched { get; private set; }

        /// <summary>
        /// add this to a table if possible
        /// </summary>
        /// <param name="game"></param>
        public PrizeTable(Games game)
        {
            prizeTable = new();
            if(game == Games.Lotto)
            {
                Dictionary<int, bool> match3 = new();
                match3.Add(3, false);
                prizeTable.Add(match3, 30);

                Dictionary<int, bool> match4 = new();
                match4.Add(4, false);
                prizeTable.Add(match4, 140);

                Dictionary<int, bool> match5 = new();
                match5.Add(5, false);
                prizeTable.Add(match5, 1750);

                Dictionary<int, bool> match5B = new();
                match5B.Add(5, true);
                prizeTable.Add(match5B, 1000000);

                Dictionary<int, bool> match6 = new();
                match6.Add(6, false);
                prizeTable.Add(match6, 2000000);
            }
            else
            {
                Dictionary<int, bool> match0B = new();
                match0B.Add(0, true);
                prizeTable.Add(match0B, 3);

                Dictionary<int, bool> match1B = new();
                match1B.Add(1, true);
                prizeTable.Add(match1B, 5);

                Dictionary<int, bool> match2B = new();
                match2B.Add(2, true);
                prizeTable.Add(match2B, 10);

                Dictionary<int, bool> match3 = new();
                match3.Add(3, false);
                prizeTable.Add(match3, 10);

                Dictionary<int, bool> match3B = new();
                match3B.Add(3, true);
                prizeTable.Add(match3B, 20);

                Dictionary<int, bool> match4 = new();
                match4.Add(4, false);
                prizeTable.Add(match4, 100);

                Dictionary<int, bool> match4B = new();
                match4B.Add(4, true);
                prizeTable.Add(match4B, 250);

                Dictionary<int, bool> match5 = new();
                match5.Add(5, false);
                prizeTable.Add(match5, 5000);

                Dictionary<int, bool> match5B = new();
                match5B.Add(5, true);
                prizeTable.Add(match5B, 500000);
            }
        }

        public int GetPrize(int[] ballsDrawn, Dictionary<int, int[]> userLines)
        {
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

            Dictionary<int, bool> key = new();
            key.Add(maxAmountMatch, bonusMatched);
            int prize = 0;
            if(prizeTable.ContainsKey(key))
            {
                prize = prizeTable[key];
            }
            BonusMatched = bonusMatched;
            AmountMatched = maxAmountMatch;

            return prize;
        }
    }
}
