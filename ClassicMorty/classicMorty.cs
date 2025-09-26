using RickVsMortyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassicMorty
{
    public class classicMorty : IMorty
    {
        public string Name => "Classic Morty";

        private readonly RandomNumberGenerator _rng;

        public classicMorty()
        {
            _rng = RandomNumberGenerator.Create();
        }

        public int HidePortalGun(int numberOfBoxes, FairRandomGenerator randomGenerator, out string hmac)
        {
            var mortyValue = GenerateRandomValue(numberOfBoxes);
            hmac = randomGenerator.GenerateHmac(mortyValue, numberOfBoxes);
            return mortyValue;
        }

        public bool ShouldRemoveBoxes(int rickFirstChoice, int portalGunBox, int n) => true;

        public int[] GetBoxesToRemove(int numberOfBoxes, int rickFirstChoice, int portalGunBox,
                                     FairRandomGenerator randomGenerator, out string hmac)
        {
            var boxesToKeep = new List<int> { rickFirstChoice };

            int boxToKeep;
            if (rickFirstChoice == portalGunBox)
            {
                var availableBoxes = Enumerable.Range(0, numberOfBoxes)
                    .Where(b => b != rickFirstChoice)
                    .ToList();
                boxToKeep = availableBoxes[GenerateRandomValue(availableBoxes.Count)];
            }
            else
            {
                boxToKeep = portalGunBox;
            }

            boxesToKeep.Add(boxToKeep);
            hmac = randomGenerator.GenerateHmac(boxToKeep, numberOfBoxes - 2);

            return Enumerable.Range(0, numberOfBoxes)
                .Where(b => !boxesToKeep.Contains(b))
                .ToArray();
        }

        public double GetWinProbabilityIfSwitch(int numberOfBoxes) =>
            (double)(numberOfBoxes - 1) / (numberOfBoxes * (numberOfBoxes - 2));

        public double GetWinProbabilityIfStay(int numberOfBoxes) => 1.0 / numberOfBoxes;

        public string GetIntroLine(int numberOfBoxes) =>
            $"Oh geez, Rick, I'm gonna hide your portal gun in one of the {numberOfBoxes} boxes, okay?";

        public string GetHidingLine() => "Okay, okay, I hid the gun. What's your guess?";
        public string GetRemovingBoxesLine() =>
            "Let's, uh, generate another value now, I mean, to select a box to keep in the game.";
        public string GetFinalChoiceLine() =>
            "You can switch your box (enter 0), or, you know, stick with it (enter 1).";
        public string GetWinLine() => "Aww man, you found your portal gun!";
        public string GetLoseLine() => "Aww man, you lost, Rick. Now we gotta go on one of *my* adventures!";
        public string GetPlayAgainLine() => "D-do you wanna play another round (y/n)?";

        private int GenerateRandomValue(int maxValue)
        {
            var bytes = new byte[4];
            _rng.GetBytes(bytes);
            var value = BitConverter.ToUInt32(bytes, 0);
            return (int)(value % maxValue);
        }
    }
}
