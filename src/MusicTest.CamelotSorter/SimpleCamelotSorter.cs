using MusicTest.CamelotSorter.Extensions;
using MusicTest.CamelotSorter.Models;

namespace MusicTest.CamelotSorter
{
    public class SimpleCamelotSorter : ICamelotSorter
    {
        private readonly int[] camelotPosibilities;

        public SimpleCamelotSorter(int[] camelotPosibilities)
        {
            if (camelotPosibilities is null)
            {
                throw new ArgumentNullException(nameof(camelotPosibilities));
            }

            if (camelotPosibilities.Length <= 0)
            {
                throw new ArgumentException($"{nameof(camelotPosibilities)} must contain elements.", nameof(camelotPosibilities));
            }

            this.camelotPosibilities = camelotPosibilities;
        }

        public Song[] SortSongs(Song[] songs)
        {
            var sortableSongs = songs.CloneList()
                                     .OrderBy(x => x.AltKey.ToInteger())
                                     .ToArray();

            for (var i = 0; i < sortableSongs.Length; i++)
            {
                if (i + 1 == sortableSongs.Length)
                {
                    break;
                }

                var currentSong = sortableSongs[i];

                var remainingSongs = sortableSongs.Skip(i + 1).ToArray();

                var allPossibilities = this.camelotPosibilities.Concat(new int[] { 1, -1 }).ToArray();

                var nextSong = currentSong.GetNextSong(remainingSongs, allPossibilities);

                if (nextSong is null)
                {
                    var thirdRuleNextSong = currentSong.IncrementUntilLimitReached(remainingSongs);
                    if (thirdRuleNextSong is null)
                    {
                        continue;
                    }
                    nextSong = thirdRuleNextSong;
                }

                var nextIndex = 0;

                for (var j = 0; j <= sortableSongs.Length; j++)
                {
                    if (sortableSongs[j].Id == nextSong.Id)
                    {
                        nextIndex = j;
                        break;
                    }
                }

                // Swap neighbor
                var currentNeighborSong = sortableSongs[i + 1];
                sortableSongs[i + 1] = nextSong;
                sortableSongs[nextIndex] = currentNeighborSong;
            }

            return sortableSongs;
        }


    }
}
