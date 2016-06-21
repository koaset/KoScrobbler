using KoScrobbler.Entities;
using KoScrobbler.Entities.LastFmApi;
using System.Collections.Generic;

namespace KoScrobbler.Interfaces
{
    public interface ILastFmScrobbler
    {
        GetSessionResponse GetMobileSession(string userName, string password);
        bool TryScrobble(List<Scrobble> scrobbles);
    }
}
