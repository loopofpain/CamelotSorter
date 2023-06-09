namespace MusicTest.CamelotSorter.Models
{
    public class MusicAltKey
    {
        private readonly int altKeyNumber;

        public MusicAltKey(int noteNumber)
        {
            if (noteNumber <= 0 || noteNumber > 12)
            {
                throw new ArgumentException("Note number must be between 1 and 2", nameof(noteNumber));
            }

            altKeyNumber = noteNumber;
        }

        public static bool operator !=(MusicAltKey a, MusicAltKey b)
        {
            return a.altKeyNumber != b.altKeyNumber;
        }

        public static bool operator ==(MusicAltKey a, MusicAltKey b)
        {
            return a.altKeyNumber == b.altKeyNumber;
        }

        public static MusicAltKey operator +(MusicAltKey a, MusicAltKey b)
        {
            var result = a.altKeyNumber + b.altKeyNumber;

            if (a.altKeyNumber == 12 && b.altKeyNumber == 12)
            {
                return new MusicAltKey(12);
            }
            else if (result <= 12)
            {
                return new MusicAltKey(result);
            }

            return new MusicAltKey(result - 12); ;
        }

        public static MusicAltKey operator -(MusicAltKey a, MusicAltKey b)
        {
            if (a.altKeyNumber == b.altKeyNumber)
            {
                return new MusicAltKey(12);
            }

            var subtraction = a.altKeyNumber - b.altKeyNumber;

            if (subtraction == 0)
            {
                return new MusicAltKey(12);
            }
            else if (subtraction < 0)
            {
                return new MusicAltKey(12 + subtraction);
            }

            return new MusicAltKey(subtraction);
        }

        public override string ToString()
        {
            return altKeyNumber.ToString();
        }

        public int ToInteger()
        {
            return altKeyNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            var musicAltKey = obj as MusicAltKey;

            return musicAltKey?.altKeyNumber == altKeyNumber;
        }

        public override int GetHashCode()
        {
            return altKeyNumber.GetHashCode();
        }
    }
}
