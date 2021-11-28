// © 2021 Tuukka Junnikkala

using System.IO;

namespace FavoriteRankerLibrary.Logic
{
    public static class FileHelper
    {
        public const string unrankedFilePath = "Unranked.txt";
        public const string rankedFilePath = "Ranked.txt";

        internal static bool ReadUnranked()
        {
            bool isSuccess = false;
            if (File.Exists(unrankedFilePath))
            {
                using var streamReader = File.OpenText(unrankedFilePath);

                // Clear the list of names and unranked entries.
                RankerLogic.Names.Clear();
                RankerLogic.Unranked.Clear();

                string line;
                while ((line = streamReader.ReadLine()) != null && RankerLogic.Unranked.Count < RankerLogic.MaximumEntries)
                {
                    // Each unique entry gets an ID number matching the index of the corresponding name in the list of names.
                    _ = RankerLogic.AddNewEntry(line);
                }

                isSuccess = true;
            }
            else
            {
                CreateUnranked();
            }
            return isSuccess;
        }

        internal static void CreateUnranked()
        {
            using var streamWriter = File.CreateText(unrankedFilePath);
        }

        internal static void AddToUnranked(string entry)
        {
            using var streamWriter = File.AppendText(unrankedFilePath);
            streamWriter.WriteLine(entry);
        }

        internal static void CreateRanked()
        {
            using var streamWriter = File.CreateText(rankedFilePath);
            for (int i = 0; i < RankerLogic.Ranked.Count; i++)
            {
                streamWriter.WriteLine($"{i + 1}. {RankerLogic.Names[RankerLogic.Ranked[i]]}");
            }
        }
    }
}
