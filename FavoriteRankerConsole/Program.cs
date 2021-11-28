// © 2021 Tuukka Junnikkala

using System;
using FavoriteRankerLibrary;
using FavoriteRankerLibrary.Logic;

namespace FavoriteRankerConsole
{
    internal static class Program
    {
        private static void Main()
        {
            // We have to create an instance of a class that implements the IUserInterface interface
            // and pass a reference to it to FavoriteRankerLibrary.
            IUserInterface ui = new ConsoleInterface();

            try
            {
                RankerLogic.Run(ui);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nThe application encountered an unhandled exception and could not continue!");
                Console.WriteLine("Error message:");
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPress any key to exit the application.");
                _ = Console.ReadKey(true);
            }
        }
    }
}
