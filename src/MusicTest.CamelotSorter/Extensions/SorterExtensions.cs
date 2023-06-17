using MusicTest.CamelotSorter.Models;

namespace MusicTest.CamelotSorter.Extensions
{
    public static class SorterExtensions
    {
        public static MusicAltKey GetTargetAltKey(this Song sortableSongs, int targetPosibility)
        {
            MusicAltKey targetAltKey;

            if (targetPosibility < 0)
            {
                var positiveAltKey = -1 * targetPosibility;
                targetAltKey = sortableSongs.AltKey - new MusicAltKey(positiveAltKey);
            }
            else
            {
                targetAltKey = sortableSongs.AltKey + new MusicAltKey(targetPosibility);
            }

            return targetAltKey;
        }
        public static Song[] CloneList(this Song[] songs)
        {
            var list = new List<Song>();

            foreach (var song in songs)
            {
                if (song is null)
                {
                    list.Add(null);
                    continue;
                }
                var clonedSong = (Song)song.Clone();
                list.Add(clonedSong);
            }

            return list.ToArray();
        }

        public static T[] InsertBetween<T>(this T[] arr, T element, T before, T after)
        {
            var list = arr.ToList();
            var beforeIndex = list.IndexOf(before);
            var afterIndex = list.IndexOf(after);

            if (beforeIndex == -1 || afterIndex == -1)
            {
                throw new ArgumentException("The specified elements were not found in the list.");
            }

            if (beforeIndex >= afterIndex)
            {
                throw new ArgumentException("The 'before' element should appear before the 'after' element in the list.");
            }

            list.Remove(element);

            list.Insert(afterIndex, element);

            return list.ToArray();
        }

        public static void Shuffle<T>(this List<T> list)
        {
            var random = new Random();

            var amountOfElemetns = list.Count;

            for (var i = amountOfElemetns - 1; i > 0; i--)
            {
                var nextField = random.Next(i + 1);
                Swap(list, i, nextField);
            }
        }

        private static void Swap<T>(List<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static Song? GetNextSong(this Song currentSong, ICollection<Song> remainingSongs, ICollection<int> camelotPosibilites)
        {
            foreach (var remainingPossibility in camelotPosibilites)
            {
                var targetAltKey = currentSong.GetTargetAltKey(remainingPossibility);

                var neighbors = remainingSongs.Where(x => x is not null && x.AltKey == targetAltKey).ToArray();

                if (neighbors.Any())
                {
                    return neighbors[0];
                }
            }

            return null;
        }
    }
}
