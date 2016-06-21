namespace KoScrobbler.Entities
{
    internal class LastFmGetSessionResponse
    {
        public LastFmSession Session { get; set; }
    }

    internal class LastFmSession
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
