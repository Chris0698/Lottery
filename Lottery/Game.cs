using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public abstract class Game
    {
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
    }
}
