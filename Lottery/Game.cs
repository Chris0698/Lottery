using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
	//adjust it so a random amount of games can be played at once
    public abstract class Game
    {
        protected int cost;
        protected int prizeMoney;
        protected Dictionary<int, int[]> userLines;
        protected int price;  //price of 1 game
        protected Games game;

        protected readonly int NUMBER_OF_BALLS;  //5 main ones, 1 bonus

        public Game(int numberOfBallsToBeDrawn, int price, Games game)
        {
            NUMBER_OF_BALLS = numberOfBallsToBeDrawn;
            this.price = price;
            this.game = game;
        }

        public int[] GenerateDraw()
        {
            int[] drawnNumbers = new int[NUMBER_OF_BALLS];
            Random random = new Random();
            int maxNumber = 0;
            if(game == Games.Thunderball)
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
            if(game == Games.Thunderball)
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
                while(!selected)
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

        protected void OrderArray(int[] array)
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

        protected void Save(int[] drawNumbers, Games game)
        {
            using (DrawContext context = new DrawContext())
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
    }
}
