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

        public string GetHidingLine() => "Yeah, yeah, it's hidden. Just pick one already.";
        public string GetRemovingBoxesLine() => "I'm gonna remove some boxes now, I guess...";
        public string GetFinalChoiceLine() => "So, like, wanna switch or stick with your box? (0=switch, 1=stay)";
        public string GetWinLine() => "Oh, you found it. Great.";
        public string GetLoseLine() => "Guess you're stuck with my boring adventures now.";
        public string GetPlayAgainLine() => "You wanna go again or what? (y/n)";

        private int GenerateRandomValue(int maxValue)
        {
            var bytes = new byte[4];
            _rng.GetBytes(bytes);
            var value = BitConverter.ToUInt32(bytes, 0);
            return (int)(value % maxValue);
        }
    }
}
