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

            var positionFirstFreeElement = 0;
            var isFixed = false;

            while (true)
            {
                var firstFreeElementInArray = onlyFixedSongs.ToList().FindIndex(positionFirstFreeElement, x => x is null);
                if (firstFreeElementInArray==-1)
                {
                    break;
                }
                var fixedElementAfterFirstFreeElement = onlyFixedSongs.ToList().FindIndex(firstFreeElementInArray, x => x is not null && x.IsFixed);

                positionFirstFreeElement = firstFreeElementInArray;

                if (firstFreeElementInArray < 0 && fixedElementAfterFirstFreeElement < 0)
                {
                    break;
                }
                else if (firstFreeElementInArray > 0 && fixedElementAfterFirstFreeElement < 0)
                {
                    fixedElementAfterFirstFreeElement = onlyFixedSongs.Length - 1;
                    isFixed = false;
                }
                else
                {
                    isFixed = true;
                }

                if (isFixed)
                {
                    var distanceBetweenStartAndEnd = fixedElementAfterFirstFreeElement - firstFreeElementInArray;
                    var halfDistance = (int)(distanceBetweenStartAndEnd / 2);
                    var lastElementToFill = firstFreeElementInArray + halfDistance;

                    if (distanceBetweenStartAndEnd % 2 > 0)
                    {
                        lastElementToFill++;
                    }


                    for (var i = firstFreeElementInArray; i <= lastElementToFill; i++)
                    {
                        if (i > 0)
                        {
                            var allPoss = this.camelotPosibilities.Concat(new int[] { 1, -1 }).ToArray();
                            var nextSong = onlyFixedSongs[i - 1].GetNextSong(availableSongs, allPoss);

                            if (nextSong is null)
                            {
                                nextSong = onlyFixedSongs[i - 1].IncrementUntilLimitReached(availableSongs);

                                if (nextSong is null)
                                {
                                    throw new Exception("");
                                }
                            }

                            availableSongs.Remove(nextSong);
                            onlyFixedSongs[i] = nextSong;
                        }
                    }

                    for (var i = fixedElementAfterFirstFreeElement-1; i > lastElementToFill; i--)
                        {
                        if (i >= onlyFixedSongs.Length - 1)
                        {
                            continue;
                        }


                        var songForNext = onlyFixedSongs[i + 1];

                        if (onlyFixedSongs[i + 1] != null)
                        {
                            songForNext = onlyFixedSongs[i + 1];
                        }else
                        {
                            throw new Exception("");
                        }


                        var nextSong = songForNext.GetNextSong(availableSongs, this.camelotPosibilities);

                        if (nextSong is null)
                        {
                            var poss = this.camelotPosibilities.Concat(new int[] { 1, -1 }).ToArray();
                            nextSong = songForNext.GetNextSong(availableSongs, poss);
                            
                            if (nextSong is null)
                            {
                                nextSong = songForNext.IncrementUntilLimitReached(availableSongs);

                                if (nextSong is null)
                                {
                                    throw new Exception("");
                                }
                            }
                        }

                        availableSongs.Remove(nextSong);
                        onlyFixedSongs[i] = nextSong;

                    }
                }
                else
                {
                    for (var i = firstFreeElementInArray; i <= fixedElementAfterFirstFreeElement; i++)
                    {
                        var poss = this.camelotPosibilities.Concat(new int[] { 1, -1 }).ToArray();
                        var nextSong = onlyFixedSongs[i - 1].GetNextSong(availableSongs, poss);

                        if (nextSong is null)
                        {
                            nextSong = onlyFixedSongs[i - 1].IncrementUntilLimitReached(availableSongs);

                            if (nextSong is null)
                            {
                                throw new Exception("");
                            }
                        }

                        availableSongs.Remove(nextSong);
                        onlyFixedSongs[i] = nextSong;
                    }
                }

                var optimizeSortedList = onlyFixedSongs.ToArray().CloneList();

                var indexFirstNotFixedEntry = optimizeSortedList.ToList().FindIndex(positionFirstFreeElement, x => x?.IsFixed == false);
                var indexNextFixedEntry = optimizeSortedList.ToList().FindIndex(indexFirstNotFixedEntry, x => x?.IsFixed == true);
                indexNextFixedEntry = indexNextFixedEntry < 0 ? optimizeSortedList.Length - 1 : indexNextFixedEntry;
                positionFirstFreeElement = indexNextFixedEntry;

                // List starting with a fixed entry and ending with a fixed array
                var trimmedListToSort = optimizeSortedList[(indexFirstNotFixedEntry - 1)..(indexNextFixedEntry+1)];
                var sortedTrimmedList = this.camelotSorter.SortSongs(trimmedListToSort);

                // Copy results from trimmed List to source list
                for (var i = indexFirstNotFixedEntry - 1; i < indexNextFixedEntry ; i++)
                {
                    var indexForTrimmedList = i - (indexFirstNotFixedEntry - 1);
                    optimizeSortedList[i] = sortedTrimmedList[indexForTrimmedList];
                }
            }
            return onlyFixedSongs;
        }

    }
}
