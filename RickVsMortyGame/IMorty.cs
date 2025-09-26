using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public interface IMorty
    {
        string Name { get; }

        int HidePortalGun(int numberOfBoxes, FairRandomGenerator randomGenerator, out string hmac);
        bool ShouldRemoveBoxes(int rickFirstChoice, int portalGunBox, int numberOfBoxes);
        int[] GetBoxesToRemove(int numberOfBoxes, int rickFirstChoice, int portalGunBox,
                              FairRandomGenerator randomGenerator, out string hmac);

        double GetWinProbabilityIfSwitch(int numberOfBoxes);
        double GetWinProbabilityIfStay(int numberOfBoxes);

        string GetIntroLine(int numberOfBoxes);
        string GetHidingLine();
        string GetRemovingBoxesLine();
        string GetFinalChoiceLine();
        string GetWinLine();
        string GetLoseLine();
        string GetPlayAgainLine();
    }

}
