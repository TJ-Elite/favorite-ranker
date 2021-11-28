// © 2021 Tuukka Junnikkala

using System.Collections.Generic;

namespace FavoriteRankerLibrary.Logic
{
    public static class RankerHelper
    {
        public static Relation GetOpposite(this Relation relation)
        {
            if (relation == Relation.Better)
            {
                return Relation.Worse;
            }
            else if (relation == Relation.Worse)
            {
                return Relation.Better;
            }
            return Relation.None;
        }

        public static string LimitNameLength(string name)
        {
            name = name.Trim();
            if (name.Length > RankerLogic.MaximumNameLength)
            {
                name = name.Remove(RankerLogic.MaximumNameLength);
            }
            return name;
        }

        internal static bool AskToPerformAction(string phraseToDisplay, List<string> commandPhrases)
        {
            RankerLogic.UI.PrintToUser(phraseToDisplay, false);
            RankerLogic.UI.PrintToUser("or press just ENTER to continue.");
            string userInput = RankerLogic.UI.GetUserInput().Trim().Trim('"').Trim('\'').ToUpper();
            foreach (string phrase in commandPhrases)
            {
                if (userInput == phrase.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool CheckIfDuplicateOrEmpty(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return true;
            }
            foreach (string name in RankerLogic.Names)
            {
                if (userInput == name)
                {
                    return true;
                }
            }
            return false;
        }

        internal static void ClearUnranked()
        {
            FileHelper.CreateUnranked();
            RankerLogic.Names.Clear();
            RankerLogic.Unranked.Clear();
            RankerLogic.UI.PrintToUser($"\n{FileHelper.unrankedFilePath} was cleared.");
        }

        internal static void DisplayRanked()
        {
            RankerLogic.UI.PrintToUser("\nFinal ranking:\n");
            for (int i = 0; i < RankerLogic.Ranked.Count; i++)
            {
                RankerLogic.UI.PrintToUser($"{i + 1}. {RankerLogic.Names[RankerLogic.Ranked[i]]}");
            }
        }

        internal static void DisplayUnranked()
        {
            RankerLogic.UI.PrintToUser("\nList of entries to be ranked:\n");
            foreach (string name in RankerLogic.Names)
            {
                RankerLogic.UI.PrintToUser(name);
            }
        }

        internal static int FindComparisonIndex(int unrankedIndex, ushort id)
        {
            for (int i = 0; i < RankerLogic.Unranked[unrankedIndex].Comparisons.Count; i++)
            {
                if (RankerLogic.Unranked[unrankedIndex].Comparisons[i].ID == id)
                {
                    return i;
                }
            }
            return -1;
        }

        internal static int FindUnrankedIndex(ushort id)
        {
            for (int i = 0; i < RankerLogic.Unranked.Count; i++)
            {
                if (RankerLogic.Unranked[i].ID == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
