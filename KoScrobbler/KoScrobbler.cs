using KoScrobbler.Interfaces;
using System;
using System.Collections.Generic;
using KoScrobbler.Entities;

namespace KoScrobbler
{
    public class KoScrobbler : ILastFmScrobbler
    {
        internal static string ApiKey;

        private LastFmApiClient lastFmClient;

        public KoScrobbler(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string GetSessionToken(string userName, string password)
        {
            var method = "auth.gettoken";

            var parameterDictionary = new Dictionary<string, string>
            {
                {"username", userName },
                {"password", password }
            };

            var client = new LastFmApiClient(method, parameterDictionary);

            var httpResponse = client.Get();

            var getTokenResponse = new GetTokenResponse(httpResponse);
            
            return getTokenResponse.Successful ? getTokenResponse.SessionToken : string.Empty;
        }

        public LastFmResponseBase TryScrobble(List<Scrobble> scrobbles)
        {
            throw new NotImplementedException();
        }
    }
}
