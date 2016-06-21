using KoScrobbler.Entities;
using System.Collections.Generic;

namespace KoScrobbler.Interfaces
{
    public interface ILastFmScrobbler
    {
        string GetMobileSession(string userName, string password);
        void TryScrobble(List<Scrobble> scrobbles);
    }
}
