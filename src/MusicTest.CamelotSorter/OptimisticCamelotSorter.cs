using MusicTest.CamelotSorter.Extensions;
using MusicTest.CamelotSorter.Models;

namespace MusicTest.CamelotSorter
{
    public class OptimisticCamelotSorter : ICamelotSorter
    {
        private readonly int[] camelotPosibilities;

        private readonly ICamelotSorter simpleCamelotSorter;

        public OptimisticCamelotSorter(int[] camelotPosibilities)
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
            this.simpleCamelotSorter = new SimpleCamelotSorter(this.camelotPosibilities);
        }

        public Song[] SortSongs(Song[] songs)
        {
            if (songs is null)
            {
                throw new ArgumentNullException(nameof(songs));
            }

            // Delete first and last song if it contains fixed songs
            var firstSong = songs.FirstOrDefault();
            var lastSong = songs.LastOrDefault();
            var isListWithFixedSongs = false;

            var songsToSort = songs.ToList();
            if (firstSong?.IsFixed == true && lastSong?.IsFixed == true)
            {
                isListWithFixedSongs = true;
                songsToSort.Remove(firstSong);
                songsToSort.Remove(lastSong);
            }

            var sortedBySimpleSorter = this.simpleCamelotSorter.SortSongs(songsToSort.ToArray());

            // Add fixed songs to list again
            if (isListWithFixedSongs)
            {
                songsToSort = sortedBySimpleSorter.ToList();
                songsToSort.Insert(0, firstSong!);
                songsToSort.Add(lastSong!);
            }

            var resultOptimized = this.OptimisticSort(songsToSort.ToArray(), isListWithFixedSongs);

            return resultOptimized;
        }

        private Song[] OptimisticSort(Song[] sortableSongs, bool isListWithFixedSongs)
        {
            var clonedSongs = sortableSongs.CloneList()
                                           .ToArray();

            for (var i = 1; i < clonedSongs.Length; i++)
            {
                if (i + 1 == clonedSongs.Length || (i - 1 <= 0 && !isListWithFixedSongs))
                {
                    continue;
                }

                var doNeighborsFulfilPosibilities = CheckIfSongsFulfillPosibilities(i, clonedSongs);

                if (!doNeighborsFulfilPosibilities)
                {
                    var songWithIssue = GetSongWithoutAnyMatchingNeighbor(i, clonedSongs, this.camelotPosibilities);
                    songWithIssue ??= clonedSongs[i + 1];

                    if (songWithIssue?.IsFixed == true)
                    {
                        continue;
                    }

                    var indexOfAncestorWithSameMusicAltKey = clonedSongs.Take(i - 1)
                                                                   .ToList()
                                                                   .FindLastIndex(x => x.AltKey.ToInteger() == songWithIssue.AltKey.ToInteger());

                    if (indexOfAncestorWithSameMusicAltKey == -1)
                    {
                        continue;
                    }

                    var ancestorWithSameMusicAltKey = clonedSongs[indexOfAncestorWithSameMusicAltKey];
                    var neighborOfAncestor = clonedSongs[indexOfAncestorWithSameMusicAltKey + 1];

                    clonedSongs = clonedSongs.InsertBetween(songWithIssue, ancestorWithSameMusicAltKey, neighborOfAncestor);
                }

            };

            if (!isListWithFixedSongs)
            {
                clonedSongs = FixLastEntryIfNeeded(clonedSongs);
            }

            return clonedSongs;
        }

        private Song[] FixLastEntryIfNeeded(Song[] songs)
        {
            var last = songs[^1];
            var secondLast = songs[^2];

            if (!CheckIfSongsFulfillPosibilities(last, secondLast, this.camelotPosibilities))
            {
                var indexOfNeighbor = songs.Take(songs.Length - 1)
                                           .ToList()
                                           .FindLastIndex(x => x.AltKey.ToInteger() == last.AltKey.ToInteger());

                if (indexOfNeighbor == -1)
                {
                    return songs;
                }

                var neighbor = songs[indexOfNeighbor];
                var neighborSuccesor = songs[indexOfNeighbor + 1];
                songs = songs.InsertBetween(last, neighbor, neighborSuccesor);
            }

            //var conditionFulfilled = CheckIfSongsFulfillPosibilities(last, secondLast, allPosibilities.ToArray());
            //if (!conditionFulfilled)
            //{
            //    var indexOfNeighbor = songs.Take(songs.Length - 1)
            //                               .ToList()
            //                               .FindLastIndex(x => x.AltKey.ToInteger() == last.AltKey.ToInteger());

            //    if (indexOfNeighbor==-1)
            //    {
            //        return songs;
            //    }

            //    var neighbor = songs[indexOfNeighbor];
            //    var neighborSuccesor = songs[indexOfNeighbor + 1];
            //    songs = songs.InsertBetween(last, neighbor, neighborSuccesor);
            //}

            return songs;
        }

        public bool CheckIfSongsFulfillPosibilities(int pos, Song[] songs)
        {
            var predecor = songs[pos - 1];
            var currentSong = songs[pos];
            var successor = songs[pos + 1];

            var firstPair = CheckIfSongsFulfillPosibilities(predecor, currentSong, this.camelotPosibilities);
            var secondPair = CheckIfSongsFulfillPosibilities(currentSong, successor, this.camelotPosibilities);

            var result = firstPair && secondPair;

            return result;
        }

        private static Song? GetSongWithoutAnyMatchingNeighbor(int currentPositionInArray, Song[] songs, int[] camelotPosibilities)
        {
            var predecessor = songs[currentPositionInArray - 1];
            var currentSong = songs[currentPositionInArray];
            var successor = songs[currentPositionInArray + 1];

            var isFirstPairOk = CheckIfSongsFulfillPosibilities(predecessor, currentSong, camelotPosibilities);
            if (isFirstPairOk)
            {
                return currentSong;
            }

            var isSecondPairOkay = CheckIfSongsFulfillPosibilities(currentSong, successor, camelotPosibilities);
            if (isSecondPairOkay)
            {
                return successor;
            }

            return null;
        }

        private static bool CheckIfSongsFulfillPosibilities(Song predecor, Song currentSong, int[] camelotPosibilities)
        {
            foreach (var posibility in camelotPosibilities)
            {
                var positivePosibility = posibility <= 0 ? posibility * -1 : posibility;
                var nextMusicAltKey = new MusicAltKey(positivePosibility);
                var predecorMusicKey = new MusicAltKey(predecor.AltKey.ToInteger());

                bool possibilityOperationEqualsCurrentSong;

                if (posibility <= 0)
                {
                    possibilityOperationEqualsCurrentSong = predecorMusicKey - nextMusicAltKey == currentSong.AltKey;
                }
                else
                {
                    possibilityOperationEqualsCurrentSong = predecorMusicKey + nextMusicAltKey == currentSong.AltKey;
                }

                if (possibilityOperationEqualsCurrentSong)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
