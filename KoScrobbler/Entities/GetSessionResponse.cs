namespace KoScrobbler.Entities
{
    public class GetSessionResponse
    {
        public Session Session { get; set; }
    }

    public class Session
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
