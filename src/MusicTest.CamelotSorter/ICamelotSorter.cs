using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MusicTest.CamelotSorter.Models;

namespace MusicTest.CamelotSorter
{
    public interface ICamelotSorter
    {
        public Song[] SortSongs(Song[] songs);
    }
}
