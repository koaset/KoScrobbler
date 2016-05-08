namespace KoScrobbler.Entities
{
    public class LastFmResponseBase
    {
        public bool Successful { get; protected set; }
        public string Message { get; protected set; }
    }
}
