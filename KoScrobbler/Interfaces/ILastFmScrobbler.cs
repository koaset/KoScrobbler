using KoScrobbler.Entities;
using System.Collections.Generic;

namespace KoScrobbler.Interfaces
{
    public interface ILastFmScrobbler
    {
        GetSessionResponse GetMobileSession(string userName, string password);
        ScrobbleResponse TryScrobble(List<Scrobble> scrobbles);
        string SessionKey { get; set; }
    }
}
