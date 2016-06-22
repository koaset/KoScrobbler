namespace KoScrobbler.Entities
{
    public class ValidateSessionResult : RequestResponseBase
    {
        public string UserName { get; internal set; }
        public string SessionKey { get; internal set; }
    }
}
