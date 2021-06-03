using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public interface IGame
    {
        public void SelectMode();

        public void CheckWinnings();

        public int [] GenerateDraw();

        public void GetStats();
    }
}
