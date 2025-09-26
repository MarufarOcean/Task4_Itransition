using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public class PortalGame
    {
        private readonly IMorty _morty;
        private readonly int _numberOfBoxes;
        private readonly FairRandomGenerator _randomGenerator;
        private readonly GameStatistics _statistics;

        public PortalGame(IMorty morty, int numberOfBoxes)
        {
            _morty = morty;
            _numberOfBoxes = numberOfBoxes;
            _randomGenerator = new FairRandomGenerator();
            _statistics = new GameStatistics();
        }

        public void PlayGame()
        {
            Console.WriteLine($"Morty: {_morty.GetIntroLine(_numberOfBoxes)}");

            var hmac1 = "";
            var portalGunBox = _morty.HidePortalGun(_numberOfBoxes, _randomGenerator, out hmac1);

            Console.WriteLine($"Morty: HMAC1={hmac1}");
            Console.WriteLine($"Morty: Rick, enter your number [0,{_numberOfBoxes}) so you don't whine later that I cheated, alright?");
            Console.Write("Rick: ");

            var rickFirstValue = ConsoleHelper.ReadIntegerInput("", 0, _numberOfBoxes);

            var secretKey1 = _randomGenerator.GenerateSecretKey();
            var firstRandom = _randomGenerator.GenerateFairRandom(portalGunBox, rickFirstValue, _numberOfBoxes, secretKey1);

            Console.WriteLine($"Morty: {string.Format(_morty.GetHidingLine(), _numberOfBoxes)}");
            Console.Write("Rick: ");

            var rickFirstChoice = ConsoleHelper.ReadIntegerInput("", 0, _numberOfBoxes);

            int rickFinalChoice = rickFirstChoice;
            bool boxesRemoved = false;

            if (_morty.ShouldRemoveBoxes(_numberOfBoxes, rickFirstChoice, portalGunBox) && _numberOfBoxes > 2)
            {
                boxesRemoved = true;
                Console.WriteLine($"Morty: {_morty.GetRemovingBoxesLine()}");

                var hmac2 = "";
                var boxesToRemove = _morty.GetBoxesToRemove(_numberOfBoxes, rickFirstChoice, portalGunBox,
                    _randomGenerator, out hmac2);

                Console.WriteLine($"Morty: HMAC2={hmac2}");
                Console.WriteLine($"Morty: Rick, enter your number [0,{_numberOfBoxes - boxesToRemove.Length}), and, uh, don't say I didn't play fair, okay?");
                Console.Write("Rick: ");

                var rickSecondValue = ConsoleHelper.ReadIntegerInput("", 0, _numberOfBoxes - boxesToRemove.Length);

                var secretKey2 = _randomGenerator.GenerateSecretKey();
                var dummyValue = 0;
                var secondRandom = _randomGenerator.GenerateFairRandom(dummyValue, rickSecondValue,
                    _numberOfBoxes - boxesToRemove.Length, secretKey2);

                var remainingBoxes = GetRemainingBoxes(rickFirstChoice, boxesToRemove);
                Console.WriteLine($"Morty: I'm keeping the box you chose, I mean {rickFirstChoice}, and the box {remainingBoxes[1]}.");

                Console.WriteLine($"Morty: {_morty.GetFinalChoiceLine()}");
                Console.Write("Rick: ");

                var switchChoice = ConsoleHelper.ReadIntegerInput("", 0, 2);
                rickFinalChoice = switchChoice == 0 ? remainingBoxes[1] : rickFirstChoice;

                Console.WriteLine($"Morty: Aww man, my 1st random value is {firstRandom.MortyValue}.");
                Console.WriteLine($"Morty: KEY1={FairRandomGenerator.BytesToHex(firstRandom.SecretKey)}");
                Console.WriteLine($"Morty: So the 1st fair number is ({rickFirstValue} + {firstRandom.MortyValue}) % {_numberOfBoxes} = {firstRandom.FinalValue}.");

                Console.WriteLine($"Morty: Aww man, my 2nd random value is {secondRandom.MortyValue}.");
                Console.WriteLine($"Morty: KEY2={FairRandomGenerator.BytesToHex(secondRandom.SecretKey)}");
                Console.WriteLine($"Morty: Uh, okay, the 2nd fair number is ({rickSecondValue} + {secondRandom.MortyValue}) % {_numberOfBoxes - boxesToRemove.Length} = {secondRandom.FinalValue}.");
            }

            bool won = rickFinalChoice == portalGunBox;
            bool switched = boxesRemoved && rickFinalChoice != rickFirstChoice;

            Console.WriteLine($"Morty: Your portal gun is in the box {portalGunBox}.");
            Console.WriteLine($"Morty: {(won ? _morty.GetWinLine() : _morty.GetLoseLine())}");

            _statistics.RecordGame(switched, won);
        }

        private int[] GetRemainingBoxes(int rickChoice, int[] boxesToRemove)
        {
            var remaining = Enumerable.Range(0, _numberOfBoxes)
                .Where(b => !boxesToRemove.Contains(b))
                .ToArray();

            if (remaining[0] != rickChoice)
            {
                var temp = remaining[0];
                remaining[0] = rickChoice;
                remaining[1] = temp;
            }

            return remaining;
        }

        public void RunGameLoop()
        {
            bool playAgain = true;

            while (playAgain)
            {
                PlayGame();
                Console.WriteLine($"Morty: {_morty.GetPlayAgainLine()}");
                playAgain = ConsoleHelper.ReadYesNoInput("Rick: ");
            }

            // Goodbye message comes FIRST
            Console.WriteLine("Morty: Okay... uh, bye!");

            // Game Stats comes AFTER goodbye message
            ConsoleHelper.DisplayStatistics(_statistics, _morty, _numberOfBoxes);
        }
    }
}
