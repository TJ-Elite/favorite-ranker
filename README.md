# favorite-ranker

C# application that can aid in putting a list of favorite albums/movies/etc. in numbered order by asking the user to pick their favorites from randomly selected pairs.

## Target Framework
The application has been written to target the .NET Core 3.1 framework.

## Description
The application starts off by asking the user to input entries that are to be ranked. These are written to a file named Unranked.txt. Alternatively the user can create said file in advance, which the application will detect when it is launched. The format for the text file is simple: each line separated by a newline is treated as one entry.

Next the application will randomly select two entries from the list and ask the user which one they like more. The user can either pick A or B or skip the question. The program will keep asking the user to rank these pairs until it has all the necessary data. Ranking data that can be infered from already provided data will not be asked from the user, reducing the number of questions required to answer.

Once the program has the final results, they will be written in a file called Ranked.txt.
