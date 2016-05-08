using KoScrobbler.Entities;
using System.Collections.Generic;

namespace KoScrobbler.Interfaces
{
    public interface ILastFmScrobbler
    {
        string GetSessionToken(string userName, string password);
        LastFmResponseBase TryScrobble(List<Scrobble> scrobbles);
    }
}
