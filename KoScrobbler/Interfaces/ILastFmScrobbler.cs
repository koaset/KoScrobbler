using KoScrobbler.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoScrobbler.Interfaces
{
    public interface ILastFmScrobbler
    {
        Task<GetSessionResult> CreateSessionAsync(string userName, string password);
        Task<ValidateSessionResult> ValidateSessionAsync(string userName, string sessionKey);
        Task<ScrobbleResult> ScrobbleAsync(List<Scrobble> scrobbles);
        string SessionKey { get; set; }
    }
}
