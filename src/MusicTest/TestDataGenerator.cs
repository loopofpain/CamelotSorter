using MusicTest.CamelotSorter.Extensions;
using MusicTest.CamelotSorter.Models;

namespace MusicTest
{
    public static class TestDataGenerator
    {
        public static Song[] GetSongs(int numberOfSongs)
        {
            var list = new List<Song>();
            var random = new Random();

            for (var i = 1; i <= numberOfSongs; i++)
            {
                var song = new Song($"Song {i}", random.Next(1, 13));
                list.Add(song);
            }

            list.Shuffle();

            var shuffledArray = list.ToArray();

            return shuffledArray;
        }
    }
}
