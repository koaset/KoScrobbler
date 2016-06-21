namespace KoScrobbler.Entities
{
    public class ScrobbleResponse : RequestResponseBase
    {
        public int SuccessfulScrobbles { get; set; }
        public int IgnoredScrobbles { get; set; }
    }
}
