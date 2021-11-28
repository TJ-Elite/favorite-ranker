// © 2021 Tuukka Junnikkala

using System;
using System.Collections.Generic;

namespace FavoriteRankerLibrary.UnitTests.Mocking
{
    public class MockInterface : IUserInterface
    {
        private const int LatestLinesCapacity = 3;

        private readonly uint _totalEntries;

        public MockInterface(uint totalEntries)
        {
            _totalEntries = totalEntries;
        }

        public bool TestFailed { get; private set; } = false;
        private uint TotalEntries
        {
            get => _totalEntries;
        }
        private int Counter1 { get; set; } = 1;
        private int Counter2 { get; set; }
        private bool RankingPrinting { get; set; }
        private List<string> LatestLines { get; set; } = new List<string>(LatestLinesCapacity);

        public void PrintToUser(string phrase)
        {
            PrintToUser(phrase, true);
        }

        public void PrintToUser(string phrase, bool newline)
        {
            if (LatestLines.Count >= LatestLinesCapacity)
            {
                LatestLines.RemoveAt(0);
            }

            if (newline)
            {
                LatestLines.Add($"{phrase}\n");
            }
            else
            {
                LatestLines.Add(phrase);
            }

            ListenForRanking();
        }

        public string GetUserInput()
        {
            switch (LatestLines[^1])
            {
                case "or press just ENTER to continue.\n":
                    if (LatestLines[^2] == "Type \"y\" or \"yes\" and press ENTER to display the list of current entries,\n")
                    {
                        return "";
                    }
                    else if (LatestLines[^2] == "\nIf you want to clear the existing list, type \"clear\" and press ENTER,\n")
                    {
                        return "clear";
                    }
                    else if (LatestLines[^2] == "If you wish to display the final ranking on screen,\ntype \"y\" or \"yes\" and press ENTER, ")
                    {
                        return "yes";
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                case ":\n":
                case ",\nor press just ENTER if you don't want to add more entries.\n":
                    if (LatestLines[^2] == "Type the name of an entry to be added to the list and press ENTER")
                    {
                        if (Counter1 <= TotalEntries)
                        {
                            return Counter1++.ToString();
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                case "\nEnter the corresponding letter and press ENTER or press just ENTER to skip this question.\n":
                    return SelectHigherNumber();

                case "Press ENTER to exit the ranker.\n":
                    return "";

                default:
                    throw new InvalidOperationException();
            }
        }

        private void ListenForRanking()
        {
            if (RankingPrinting && !TestFailed)
            {
                if (LatestLines[^1] == "\n")
                {
                    RankingPrinting = false;
                }
                else if (LatestLines[^1] != $"{Counter1}. {Counter2}\n")
                {
                    TestFailed = true;
                }
                ++Counter1;
                --Counter2;
            }
            else if (LatestLines[^1] == "\nFinal ranking:\n\n")
            {
                RankingPrinting = true;
                Counter1 = 1;
                Counter2 = (int)TotalEntries;
            }
        }

        private string SelectHigherNumber()
        {
            if (int.Parse(LatestLines[^3][3..].TrimEnd('\n')) > int.Parse(LatestLines[^2][3..].TrimEnd('\n')))
            {
                return "A";
            }
            else if (int.Parse(LatestLines[^2][3..].TrimEnd('\n')) > int.Parse(LatestLines[^3][3..].TrimEnd('\n')))
            {
                return "B";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
