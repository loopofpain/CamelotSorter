// See https://aka.ms/new-console-template for more information
using MusicTest;
using MusicTest.CamelotSorter;

var camelotPossibilities = new int[]
{
    2,
    -2,
    //3,
    //-3,
    7,
    -7
};
var allSongs = TestDataGenerator.GetSongs(64);

ICamelotSorter optimisticSorter = new OptimisticCamelotSorter(camelotPossibilities);
var optimisticSortedSongs = optimisticSorter.SortSongs(allSongs);

Console.WriteLine("Finished sorting!");
