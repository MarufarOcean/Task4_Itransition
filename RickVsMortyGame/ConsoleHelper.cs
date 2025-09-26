using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public class ConsoleHelper
    {
        public static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static int ReadIntegerInput(string prompt, int minValue, int maxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result >= minValue && result < maxValue)
                {
                    return result;
                }

                Console.WriteLine($"Please enter a number between {minValue} and {maxValue - 1}.");
            }
        }

        public static bool ReadYesNoInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = (Console.ReadLine() ?? "").Trim().ToLower();

                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;

                Console.WriteLine("Please enter 'y' for yes or 'n' for no.");
            }
        }

        public static void DisplayStatistics(GameStatistics stats, IMorty morty, int numberOfBoxes)
        {
            Console.WriteLine();
            Console.WriteLine("                  GAME STATS ");
            Console.WriteLine("┌──────────────┬───────────────┬─────────────┐");
            Console.WriteLine("│ Game results │ Rick switched │ Rick stayed │");
            Console.WriteLine("├──────────────┼───────────────┼─────────────┤");

            Console.WriteLine($"│ Rounds       │ {stats.SwitchedRounds,13} │ {stats.StayedRounds,11} │");
            Console.WriteLine($"│ Wins         │ {stats.SwitchedWins,13} │ {stats.StayedWins,11} │");

            var switchedEstimate = stats.GetEstimatedWinProbability(true).ToString("F3");
            var stayedEstimate = stats.GetEstimatedWinProbability(false).ToString("F3");
            Console.WriteLine($"│ P (estimate) │ {switchedEstimate,13} │ {stayedEstimate,11} │");

            var switchedExact = morty.GetWinProbabilityIfSwitch(numberOfBoxes).ToString("F3");
            var stayedExact = morty.GetWinProbabilityIfStay(numberOfBoxes).ToString("F3");
            Console.WriteLine($"│ P (exact)    │ {switchedExact,13} │ {stayedExact,11} │");

            Console.WriteLine("└──────────────┴───────────────┴─────────────┘");
        }
    }
}
