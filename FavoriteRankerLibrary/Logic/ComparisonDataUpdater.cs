// © 2021 Tuukka Junnikkala

using System.Collections.Generic;

namespace FavoriteRankerLibrary.Logic
{
    internal static class ComparisonDataUpdater
    {
        internal static void Update(int winner, int loser)
        {
            // Update the two entries that were compared and also update any comparison data that can be logically deduced from
            // current data.
            UpdateEntryPair(winner, loser, Relation.Better);
            // Go through the list of unranked entries and see if any of them have been fully ranked and move such entries to ranked.
            CheckFullyRanked();
        }

        private static void UpdateEntryPair(int firstEntry, int counterpart, Relation relation)
        {
            // Update first entry.
            UpdateEntry(firstEntry, counterpart, relation);
            // Update counterpart.
            UpdateEntry(counterpart, firstEntry, relation.GetOpposite());
        }

        private static void UpdateEntry(int entry, int counterpart, Relation relation)
        {
            int comparisonIndex = RankerHelper.FindComparisonIndex(entry, RankerLogic.Unranked[counterpart].ID);
            RankerLogic.Unranked[entry].Comparisons[comparisonIndex].Comparison = relation;
            InheritFromOtherEntry(entry, counterpart, relation);
        }

        private static void InheritFromOtherEntry(int thisEntry, int otherEntry, Relation relation)
        {
            for (int i = 0; i < RankerLogic.Unranked[thisEntry].Comparisons.Count; i++)
            {
                for (int j = 0; j < RankerLogic.Unranked[otherEntry].Comparisons.Count; j++)
                {
                    if (RankerLogic.Unranked[thisEntry].Comparisons[i].ID == RankerLogic.Unranked[otherEntry].Comparisons[j].ID &&
                        RankerLogic.Unranked[thisEntry].Comparisons[i].Comparison == Relation.None &&
                        RankerLogic.Unranked[otherEntry].Comparisons[j].Comparison == relation)
                    {
                        UpdateEntryPair(
                            thisEntry,
                            RankerHelper.FindUnrankedIndex(RankerLogic.Unranked[thisEntry].Comparisons[i].ID),
                            relation);
                        break;
                    }
                }
            }
        }

        private static void CheckFullyRanked()
        {
            var readyToMove = new List<int>();
            for (int i = 0; i < RankerLogic.Unranked.Count; i++)
            {
                bool fullyRanked = true;
                foreach (var comparison in RankerLogic.Unranked[i].Comparisons)
                {
                    if (comparison.Comparison == Relation.None)
                    {
                        fullyRanked = false;
                        break;
                    }
                }
                // Queue the entry for move from unranked to ranked if its comparison data is complete.
                if (fullyRanked)
                {
                    readyToMove.Add(i);
                }
            }
            // Move all fully ranked entries from unranked to ranked, starting from the highest index to avoid wrong behavior.
            while (readyToMove.Count > 0)
            {
                MoveToRanked(readyToMove[^1]);
                readyToMove.RemoveAt(readyToMove.Count - 1);
            }
        }

        private static void MoveToRanked(int index)
        {
            if (RankerLogic.Ranked.Count > 0)
            {
                for (int i = 0; i < RankerLogic.Ranked.Count; i++)
                {
                    for (int j = 0; j < RankerLogic.Unranked[index].Comparisons.Count; j++)
                    {
                        if (RankerLogic.Unranked[index].Comparisons[j].ID == RankerLogic.Ranked[i])
                        {
                            if (RankerLogic.Unranked[index].Comparisons[j].Comparison == Relation.Better)
                            {
                                RankerLogic.Ranked.Insert(i, RankerLogic.Unranked[index].ID);
                                RankerLogic.Unranked.RemoveAt(index);
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            // Add the entry at the end of the ranked list if the list is empty
            // or the entry isn't better than any entry already on the list.
            RankerLogic.Ranked.Add(RankerLogic.Unranked[index].ID);
            RankerLogic.Unranked.RemoveAt(index);
        }
    }
}
