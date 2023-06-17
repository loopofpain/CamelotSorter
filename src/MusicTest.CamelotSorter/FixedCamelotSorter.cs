using MusicTest.CamelotSorter.Extensions;
using MusicTest.CamelotSorter.Models;

namespace MusicTest.CamelotSorter
{
    public class FixedCamelotSorter : ICamelotSorter
    {
        private readonly int[] camelotPosibilities;
        private readonly ICamelotSorter camelotSorter;

        public FixedCamelotSorter(int[] camelotPosibilities)
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

            this.camelotSorter = new OptimisticCamelotSorter(this.camelotPosibilities);
        }

        public Song[] SortSongs(Song[] songs)
        {
            if (songs is null)
            {
                throw new ArgumentNullException(nameof(songs));
            }

            var resultOptimized = this.Sort(songs);

            return resultOptimized;
        }

        private Song[] Sort(Song[] sortableSongs)
        {
            var availableSongs = sortableSongs.CloneList()
                                              .Where(x => !x.IsFixed)
                                              .ToList();
            var onlyFixedSongs = sortableSongs.CloneList()
                                              .ToArray();

            for (var i = 0; i < onlyFixedSongs.Length; i++)
            {
                if (!onlyFixedSongs[i].IsFixed)
                {
                    onlyFixedSongs[i] = null;
                }
            }         

            var positionOfFirstFreeElement = -1;
            var positionOfLastFreeElement = -1;

            for (var i = 0; i < onlyFixedSongs.Length; i++)
            {
                if (positionOfFirstFreeElement >=0 && positionOfLastFreeElement >= 0)
                {
                    break;
                }

                if (onlyFixedSongs[i]==null && positionOfFirstFreeElement==-1)
                {
                    positionOfFirstFreeElement = i;
                    continue;
                }

                if (onlyFixedSongs[i] != null && positionOfFirstFreeElement != -1)
                {
                    positionOfLastFreeElement = i-1;
                    continue;
                }
            }

            var loop1Start = positionOfFirstFreeElement;
            var distance = positionOfLastFreeElement - loop1Start;
            var halfDistance = (int)(distance / 2);
            var loop1End = loop1Start + halfDistance;

            if (distance%2 > 0)
            {
                loop1End++;
            }


            for (var i = loop1Start; i <= loop1End; i++)
            {
                if (i > 0)
                {
                    var nextSong = onlyFixedSongs[i-1].GetNextSong(availableSongs, this.camelotPosibilities);

                    if(nextSong is null)
                    {
                        continue;
                    }

                    availableSongs.Remove(nextSong);
                    onlyFixedSongs[i] = nextSong;
                }
            }

            for (var i = positionOfLastFreeElement + 1 ; i > loop1End; i--)
            {
                if (i < onlyFixedSongs.Length-1)
                {
                    var nextSong = onlyFixedSongs[i + 1].GetNextSong(availableSongs, this.camelotPosibilities);

                    if (nextSong is null)
                    {
                        continue;
                    }

                    availableSongs.Remove(nextSong);
                    onlyFixedSongs[i] = nextSong;
                }
            }

            var optimizeSortedList = onlyFixedSongs.ToArray().CloneList();

            var indexFirstNotFixedEntry = optimizeSortedList.ToList().FindIndex(x => x?.IsFixed == false);
            var indexNextFixedEntry = optimizeSortedList.ToList().FindIndex(indexFirstNotFixedEntry, x => x?.IsFixed == true);
            
            // List starting with a fixed entry and ending with a fixed array
            var trimmedListToSort = optimizeSortedList[(indexFirstNotFixedEntry-1)..(indexNextFixedEntry+1)];
            var sortedTrimmedList = this.camelotSorter.SortSongs(trimmedListToSort);

            // Copy results from trimmed List to source list
            for (var i = indexFirstNotFixedEntry-1; i <= indexNextFixedEntry; i++)
            {
                var indexForTrimmedList = i - (indexFirstNotFixedEntry - 1);
                optimizeSortedList[i] = sortedTrimmedList[indexForTrimmedList];
            }

            return onlyFixedSongs;
        }

    }
}
