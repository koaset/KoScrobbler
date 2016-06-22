namespace KoScrobbler.Entities
{
    public class ScrobbleResult : RequestResponseBase
    {
        public int SuccessfulScrobbles { get; internal set; }
        public int IgnoredScrobbles { get; internal set; }
    }
}
