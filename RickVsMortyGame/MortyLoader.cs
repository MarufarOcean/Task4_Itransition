using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public class MortyLoader
    {
        public static IMorty LoadMorty(string assemblyPath, string className)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException($"Morty assembly not found: {assemblyPath}");
            }

            try
            {
                // Load the assembly
                var assembly = Assembly.LoadFrom(assemblyPath);

                // Find the Morty type
                var mortyType = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == className && typeof(IMorty).IsAssignableFrom(t));

                if (mortyType == null)
                {
                    throw new TypeLoadException($"Could not find Morty class '{className}' in assembly {assemblyPath}");
                }

                // Create instance
                var morty = Activator.CreateInstance(mortyType) as IMorty;
                if (morty == null)
                {
                    throw new InvalidOperationException($"Failed to create instance of {className}");
                }

                return morty;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error loading Morty implementation: {ex.Message}", ex);
            }
        }
    }
}
