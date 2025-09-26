using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public class CommandLineArgs
    {
        public int NumberOfBoxes { get; }
        public string MortyAssemblyPath { get; }
        public string MortyClassName { get; }

        public CommandLineArgs(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException(
                    "Error: Insufficient arguments provided.\n" +
                    "Usage: randm.exe <number_of_boxes> <morty_assembly_path> [morty_class_name]\n" +
                    "Example: randm.exe 3 ./ClassicMorty.dll ClassicMorty\n" +
                    "Example: randm.exe 5 ./morties/LazyMorty.dll");
            }

            if (!int.TryParse(args[0], out int boxes) || boxes < 3)
            {
                throw new ArgumentException(
                    "Error: Number of boxes must be an integer greater than 2.\n" +
                    "Example: randm.exe 3 ./ClassicMorty.dll ClassicMorty");
            }
            NumberOfBoxes = boxes;

            MortyAssemblyPath = args[1];

            if (args.Length > 2)
            {
                MortyClassName = args[2];
            }
            else
            {
                // Extract class name from filename if not provided
                var fileName = Path.GetFileNameWithoutExtension(args[1]);
                MortyClassName = fileName.Replace("Morty", "") + "Morty";
            }
        }

        public static void DisplayUsage()
        {
            Console.WriteLine("Usage: randm.exe <number_of_boxes> <morty_assembly_path> [morty_class_name]");
            Console.WriteLine("Example: randm.exe 3 ./ClassicMorty.dll ClassicMorty");
            Console.WriteLine("Example: randm.exe 5 ./morties/LazyMorty.dll LazyMorty");
        }
    }
}
