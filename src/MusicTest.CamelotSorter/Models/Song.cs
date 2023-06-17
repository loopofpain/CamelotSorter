namespace MusicTest.CamelotSorter.Models
{
    public class Song : ICloneable
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Title { get; set; }

        public MusicAltKey AltKey { get; set; }

        public bool IsFixed { get; set; }


        public Song(string title, int altKey, bool isFixed = false)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
            }

            if (altKey <= 0 || altKey > 12)
            {
                throw new ArgumentException($"'{nameof(altKey)}' must be between 1 and 12.", nameof(title));
            }

            Title = title;
            AltKey = new MusicAltKey(altKey);
            IsFixed = isFixed;
        }

        public Song(string title, int altKey, Guid id, bool isFixed = false)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
            }

            if (altKey <= 0 || altKey > 12)
            {
                throw new ArgumentException($"'{nameof(altKey)}' must be between 1 and 12.", nameof(title));
            }

            Title = title;
            AltKey = new MusicAltKey(altKey);
            Id = id;
            IsFixed = isFixed;
        }

        public override string ToString()
        {
            var isfixed = this.IsFixed ? "[FIX]" : "";
            return $"{isfixed} ({AltKey.ToInteger()} Key) (Title: {Title})";
        }

        public object Clone()
        {
            var isfixed = IsFixed ? 1 : 0;
            var fixedInstance = isfixed == 1;

            var song = new Song($"{Title}", AltKey.ToInteger(), Guid.Parse(Id.ToString()), fixedInstance)
            {
            };

            return song;
        }
    }
}
