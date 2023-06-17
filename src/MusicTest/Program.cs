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

ICamelotSorter optimisticSorter = new OptimisticCamelotSorter(camelotPossibilities);
var optimisticSortedSongs = optimisticSorter.SortSongs(allSongs);

var fixedSongs = optimisticSortedSongs.CloneList()
                                      .ToList();
var songsToFix = fixedSongs.Where(x => x.AltKey == new MusicAltKey(3))
                           .ToList();

foreach (var fixableSong in songsToFix)
{
    fixedSongs.Remove(fixableSong);
    fixableSong.IsFixed = true;
    fixedSongs.Insert(0, fixableSong);
}

fixedSongs[31].IsFixed = true;
fixedSongs[32].IsFixed = true;

fixedSongs[55].IsFixed = true;
fixedSongs[56].IsFixed = true;

ICamelotSorter fixedSorter = new FixedCamelotSorter(camelotPossibilities);
var fixSort = fixedSorter.SortSongs(fixedSongs.ToArray());


Console.WriteLine("Finished sorting!");
