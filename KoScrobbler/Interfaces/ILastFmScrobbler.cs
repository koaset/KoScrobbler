using KoScrobbler.Entities;
using System.Collections.Generic;

namespace KoScrobbler.Interfaces
{
    public interface ILastFmScrobbler
    {
        GetSessionResult CreateSession(string userName, string password);
        ValidateSessionResult ValidateSession(string userName, string sessionKey);
        ScrobbleResult TryScrobble(List<Scrobble> scrobbles);
        string SessionKey { get; set; }
    }
}
