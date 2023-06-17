// See https://aka.ms/new-console-template for more information
using MusicTest;
using MusicTest.CamelotSorter;
using MusicTest.CamelotSorter.Extensions;
using MusicTest.CamelotSorter.Models;

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

//ICamelotSorter optimisticSorter = new OptimisticCamelotSorter(camelotPossibilities);
//var optimisticSortedSongs = optimisticSorter.SortSongs(allSongs);

//var fixedSongs = optimisticSortedSongs.CloneList()
//                                      .ToList();
var songsForFixedSorter = allSongs.CloneList().ToList();

var songsToFix = songsForFixedSorter.Where(x => x.AltKey == new MusicAltKey(3))
                                    .ToList();

foreach (var fixableSong in songsToFix)
{
    songsForFixedSorter.Remove(fixableSong);
    fixableSong.IsFixed = true;
    songsForFixedSorter.Insert(0, fixableSong);
}

songsForFixedSorter[31].IsFixed = true;
songsForFixedSorter[32].IsFixed = true;

songsForFixedSorter[55].IsFixed = true;
songsForFixedSorter[56].IsFixed = true;

ICamelotSorter fixedSorter = new FixedCamelotSorter(camelotPossibilities);
var fixSort = fixedSorter.SortSongs(songsForFixedSorter.ToArray());

Console.WriteLine("Finished sorting!");
