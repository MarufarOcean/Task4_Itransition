using RickVsMortyGame;

namespace RickAndMortyGame
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    CommandLineArgs.DisplayUsage();
                    return 0;
                }

                var commandLineArgs = new CommandLineArgs(args);
                var morty = MortyLoader.LoadMorty(commandLineArgs.MortyAssemblyPath, commandLineArgs.MortyClassName);

                Console.WriteLine($"Loaded Morty: {morty.Name}");
                Console.WriteLine($"Starting game with {commandLineArgs.NumberOfBoxes} boxes...");
                Console.WriteLine();

                var game = new PortalGame(morty, commandLineArgs.NumberOfBoxes);
                game.RunGameLoop();

                return 0;
            }
            catch (Exception ex)
            {
                ConsoleHelper.DisplayError($"Error: {ex.Message}");
                Console.WriteLine();
                CommandLineArgs.DisplayUsage();
                return 1;
            }
        }
    }
}