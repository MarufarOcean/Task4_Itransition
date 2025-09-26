using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public class GameStatistics
    {
        public int TotalRounds { get; private set; }
        public int SwitchedRounds { get; private set; }
        public int StayedRounds { get; private set; }
        public int SwitchedWins { get; private set; }
        public int StayedWins { get; private set; }

        public void RecordGame(bool switched, bool won)
        {
            TotalRounds++;

            if (switched)
            {
                SwitchedRounds++;
                if (won) SwitchedWins++;
            }
            else
            {
                StayedRounds++;
                if (won) StayedWins++;
            }
        }

        public double GetEstimatedWinProbability(bool switched)
        {
            if (switched && SwitchedRounds > 0)
                return (double)SwitchedWins / SwitchedRounds;
            if (!switched && StayedRounds > 0)
                return (double)StayedWins / StayedRounds;
            return 0.0;
        }
    }
}
