// © 2021 Tuukka Junnikkala

using System;
using System.Collections.Generic;
using FavoriteRankerLibrary.Models;

namespace FavoriteRankerLibrary.Logic
{
    public static class RankerLogic
    {
        public const ushort MaximumEntries = 200;
        public const int MaximumNameLength = 128;

        private static IUserInterface _ui;

        internal static IUserInterface UI
        {
            get => _ui;
            private set => _ui = value ?? throw new ArgumentNullException(nameof(value), $"{nameof(UI)} cannot be null!");
        }
        internal static List<string> Names { get; set; } = new List<string>();
        internal static List<FavoriteModel> Unranked { get; set; } = new List<FavoriteModel>();
        internal static List<ushort> Ranked { get; set; } = new List<ushort>();

        public static void Run(IUserInterface ui)
        {
            UI = ui;

            // Try to read pre-existing list of entries from a file and create a new empty file if one does not exist.
            ReadUnrankedFile();

            // Ask the user if they wish to view the entries currently in the list as well as give them the option to
            // clear the existing list.
            AskToDisplayOrClearUnranked();

            // Ask the user to add more entries if there are less than two and keeps asking for more until the user
            // doesn't want to add any more or the maximum number of entries has been reached.
            AskForMoreEntries();

            // Initialize comparison data now that all entries are known.
            InitializeComparisons();

            // Keep asking the user to rank two randomly selected entries in relation to one another until all entries
            // have been fully ranked.
            while (Unranked.Count != 0)
            {
                RankNext();
            }

            // Write the list of ranked entries and ask to display them to the user.
            WriteRankedAndAskToDisplay();

            UI.PrintToUser("Press ENTER to exit the ranker.");
            _ = UI.GetUserInput();
        }

        internal static bool AddNewEntry(string name)
        {
            return AddNewEntry(name, false);
        }

        private static bool AddNewEntry(string name, bool addToFile)
        {
            bool entryAdded = false;
            name = RankerHelper.LimitNameLength(name);
            if (!RankerHelper.CheckIfDuplicateOrEmpty(name))
            {
                if (addToFile)
                {
                    FileHelper.AddToUnranked(name);
                }
                // We add the new entry to the unranked list first and after that to the list containing all the names.
                // When done in this order the ID numbers match the zero-based names list indexing.
                Unranked.Add(new FavoriteModel((ushort)Names.Count));
                Names.Add(name);
                entryAdded = true;
            }
            return entryAdded;
        }

        private static void ReadUnrankedFile()
        {
            if (FileHelper.ReadUnranked())
            {
                UI.PrintToUser($"{FileHelper.unrankedFilePath} was read successfully", false);
                if (Names.Count < MaximumEntries)
                {
                    UI.PrintToUser($".\n{Names.Count} entries were found.");
                }
                else
                {
                    UI.PrintToUser($",\nbut the number of unique entries was limited to the first {MaximumEntries}.");
                }
            }
            else
            {
                UI.PrintToUser($"{FileHelper.unrankedFilePath} couldn't be found so it was created.");
            }
            UI.PrintToUser("");
        }

        private static void AskToDisplayOrClearUnranked()
        {
            if (Names.Count > 0)
            {
                if (RankerHelper.AskToPerformAction(
                    "Type \"y\" or \"yes\" and press ENTER to display the list of current entries,\n",
                    new List<string> { "y", "yes" }))
                {
                    RankerHelper.DisplayUnranked();
                }

                if (RankerHelper.AskToPerformAction(
                    "\nIf you want to clear the existing list, type \"clear\" and press ENTER,\n",
                    new List<string> { "clear" }))
                {
                    RankerHelper.ClearUnranked();
                }

                UI.PrintToUser("");
            }
        }

        /// <summary>
        /// Asks the user to input more names if there are less than two in the unranked list. If the maximum number of entries hasn't been
        /// reached yet, the user can also keep adding names if they want.
        /// </summary>
        private static void AskForMoreEntries()
        {
            if (Names.Count < MaximumEntries)
            {
                // User input variable mustn't start empty.
                string userInput = "x";
                while (Names.Count < 2 || (Names.Count < MaximumEntries && !string.IsNullOrWhiteSpace(userInput)))
                {
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        UI.PrintToUser("You didn't enter anything!");
                    }
                    UI.PrintToUser($"There are currently {Names.Count} entries in the list.");
                    UI.PrintToUser("Type the name of an entry to be added to the list and press ENTER", false);
                    if (Names.Count < 2)
                    {
                        UI.PrintToUser(":");
                    }
                    else
                    {
                        UI.PrintToUser(",\nor press just ENTER if you don't want to add more entries.");
                    }

                    userInput = UI.GetUserInput();
                    UI.PrintToUser("");

                    // Add the entry to the list if it isn't empty or a duplicate.
                    if (!AddNewEntry(userInput, addToFile: true) && !string.IsNullOrWhiteSpace(userInput))
                    {
                        UI.PrintToUser("The name you entered is already on the list!\n");
                    }
                }
                if (Names.Count >= MaximumEntries)
                {
                    UI.PrintToUser($"The maximum number of {MaximumEntries} entries has been reached.\n");
                }
            }
        }

        private static void InitializeComparisons()
        {
            // Clear any prior comparison data.
            Ranked.Clear();
            for (int i = 0; i < Unranked.Count; i++)
            {
                Unranked[i].Comparisons.Clear();
            }
            // Initialize comparison data.
            for (int i = 0; i < Unranked.Count; i++)
            {
                for (int j = 0; j < Unranked.Count; j++)
                {
                    if (Unranked[i].ID != Unranked[j].ID)
                    {
                        Unranked[j].Comparisons.Add(new ComparisonModel(Unranked[i].ID));
                    }
                }
            }
        }

        private static void RankNext()
        {
            if (Unranked.Count > 1)
            {
                AskToRank(out int index1, out int index2, out string userInput);

                if (userInput == "A")
                {
                    ComparisonDataUpdater.Update(winner: index1, loser: index2);
                }
                else
                {
                    ComparisonDataUpdater.Update(winner: index2, loser: index1);
                }
            }
            else
            {
                throw new InvalidOperationException("Attempted to ask to rank when there are less than two items left in the list!");
            }
        }

        private static void AskToRank(out int index1, out int index2, out string userInput)
        {
            // Instantiate random number generator using system-supplied value as seed.
            var random = new Random();

            do
            {
                // Pick a random entry from the list of entries that haven't been fully ranked yet.
                index1 = random.Next(Unranked.Count);

                // Collect entries that haven't been compared with the randomly selected one yet into a temporary list.
                var notCompared = new List<ushort>();
                for (int i = 0; i < Unranked[index1].Comparisons.Count; i++)
                {
                    if (Unranked[index1].Comparisons[i].Comparison == Relation.None)
                    {
                        notCompared.Add(Unranked[index1].Comparisons[i].ID);
                    }
                }

                // Pick a random entry from the temporary list.
                index2 = random.Next(notCompared.Count);
                index2 = RankerHelper.FindUnrankedIndex(notCompared[index2]);

                UI.PrintToUser("Which of these two do you like more?");
                UI.PrintToUser($"A: {Names[Unranked[index1].ID]}");
                UI.PrintToUser($"B: {Names[Unranked[index2].ID]}");
                UI.PrintToUser("\nEnter the corresponding letter and press ENTER or press just ENTER to skip this question.");
                userInput = UI.GetUserInput().Trim().Trim('"').Trim('\'').ToUpper();
                while (userInput != "A" && userInput != "B" && !string.IsNullOrWhiteSpace(userInput))
                {
                    UI.PrintToUser("\nYour entry wasn't valid!");
                    UI.PrintToUser("Please input your answer again.");
                    userInput = UI.GetUserInput().Trim().Trim('"').Trim('\'').ToUpper();
                }
                UI.PrintToUser("");
            } while (string.IsNullOrWhiteSpace(userInput));
        }

        private static void WriteRankedAndAskToDisplay()
        {
            FileHelper.CreateRanked();
            UI.PrintToUser($"{FileHelper.rankedFilePath} was created with the list of ranked entries in descending order.\n");

            if (RankerHelper.AskToPerformAction(
                "If you wish to display the final ranking on screen,\ntype \"y\" or \"yes\" and press ENTER, ",
                new List<string> { "y", "yes" }))
            {
                RankerHelper.DisplayRanked();
            }
            UI.PrintToUser("");
        }
    }
}
