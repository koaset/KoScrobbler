using System;

namespace KoScrobbler.Entities
{
    public class Scrobble
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public DateTime TimePlayed { get; set; }

        public Scrobble (string artist, string album, string title, DateTime timePlayed)
        {
            Title = title;
            Artist = artist;
            Album = album;
            TimePlayed = timePlayed;
        }
    }
}
