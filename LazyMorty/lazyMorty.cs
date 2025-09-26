using RickVsMortyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LazyMorty
{
    public class lazyMorty : IMorty
    {
        public string Name => "Lazy Morty";
        private readonly RandomNumberGenerator _rng;

        public lazyMorty()
        {
            _rng = RandomNumberGenerator.Create();
        }

        public int HidePortalGun(int numberOfBoxes, FairRandomGenerator randomGenerator, out string hmac)
        {
            var mortyValue = GenerateRandomValue(numberOfBoxes);
            hmac = randomGenerator.GenerateHmac(mortyValue, numberOfBoxes);
            return mortyValue;
        }

        public bool ShouldRemoveBoxes(int numberOfBoxes, int rickFirstChoice, int portalGunBox)
        {
            return numberOfBoxes > 2; // Now we have the parameter!
        }

        public int[] GetBoxesToRemove(int numberOfBoxes, int rickFirstChoice, int portalGunBox,
                                     FairRandomGenerator randomGenerator, out string hmac)
        {
            var boxesToKeep = new List<int> { rickFirstChoice };

            // Always keep the portal gun box if Rick didn't choose it
            if (rickFirstChoice != portalGunBox)
            {
                boxesToKeep.Add(portalGunBox);
            }
            else
            {
                // If Rick chose correctly, keep a random box
                var availableBoxes = Enumerable.Range(0, numberOfBoxes)
                    .Where(b => b != rickFirstChoice)
                    .ToList();
                var randomBox = availableBoxes[GenerateRandomValue(availableBoxes.Count)];
                boxesToKeep.Add(randomBox);
            }

            hmac = randomGenerator.GenerateHmac(boxesToKeep[1], numberOfBoxes - 2);

            return Enumerable.Range(0, numberOfBoxes)
                .Where(b => !boxesToKeep.Contains(b))
                .ToArray();
        }

        public double GetWinProbabilityIfSwitch(int numberOfBoxes) =>
            numberOfBoxes > 2 ? (double)(numberOfBoxes - 1) / (numberOfBoxes) : 0.5;

        public double GetWinProbabilityIfStay(int numberOfBoxes) => 1.0 / numberOfBoxes;

        public string GetIntroLine(int numberOfBoxes) =>
            $"Uh, Rick, I guess I'll hide your portal gun in one of these {numberOfBoxes} boxes...";

        public string GetHidingLine() => "Yeah, yeah, it's hidden. Just pick one already [0,{0}).";
        public string GetRemovingBoxesLine() =>
            "Let's, uh, generate another value now, I mean, to select a box to keep in the game.";
        public string GetFinalChoiceLine() =>
            "You can switch your box (enter 0), or, you know, stick with it (enter 1).";
        public string GetWinLine() => "Aww man, you won, Rick! You found your portal gun!";
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
