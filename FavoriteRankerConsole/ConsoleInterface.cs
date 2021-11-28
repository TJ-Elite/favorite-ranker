// © 2021 Tuukka Junnikkala

using System;
using FavoriteRankerLibrary;

namespace FavoriteRankerConsole
{
    /// <summary>
    /// Implements the IUserInterface interface specified in FavoriteRankerLibrary.
    /// </summary>
    public class ConsoleInterface : IUserInterface
    {
        /// <summary>
        /// Prints information to the user with a newline at the end.
        /// </summary>
        /// <param name="phrase"></param>
        public void PrintToUser(string phrase)
        {
            PrintToUser(phrase, true);
        }

        /// <summary>
        /// Prints information to the user.
        /// </summary>
        /// <param name="phrase">The string to be printed.</param>
        /// <param name="newline">Specifies whether a newline should be added to the end or not.</param>
        public void PrintToUser(string phrase, bool newline)
        {
            if (newline)
            {
                Console.WriteLine(phrase);
            }
            else
            {
                Console.Write(phrase);
            }
        }

        /// <summary>
        /// Reads data from the user.
        /// </summary>
        /// <returns>The data provided by the user.</returns>
        public string GetUserInput()
        {
            return Console.ReadLine();
        }
    }
}
