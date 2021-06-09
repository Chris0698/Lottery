using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    //this interface is not being used right..
    public interface IGame
    {

        public void SelectMode();

        public void CheckWinnings(int[] ballsDrawn, Dictionary<int, int[]> userLines, int totalCost);

        //public int [] GenerateDraw();

        public void Play();

        public void GetStats();

        public void Search();
    }
}
