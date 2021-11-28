// © 2021 Tuukka Junnikkala

namespace FavoriteRankerLibrary
{
    /// <summary>
    /// An interface which the application using this library must implement so that the library can communicate with the user.
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        /// Prints information to the user. Should always print a newline at the end.
        /// </summary>
        /// <param name="phrase">The string to be printed.</param>
        void PrintToUser(string phrase);

        /// <summary>
        /// Prints information to the user.
        /// </summary>
        /// <param name="phrase">The string to be printed.</param>
        /// <param name="newline">Specifies whether a newline should be added to the end or not.</param>
        void PrintToUser(string phrase, bool newline);

        /// <summary>
        /// Reads data from the user.
        /// </summary>
        /// <returns>The data provided by the user.</returns>
        string GetUserInput();
    }
}
