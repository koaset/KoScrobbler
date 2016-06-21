using KoScrobbler.Interfaces;
using System;
using System.Collections.Generic;
using KoScrobbler.Entities;
using KoScrobbler.Entities.Exceptions;

namespace KoScrobbler
{
    public class KoScrobbler : ILastFmScrobbler
    {
        internal static string ApiKey;
        internal static string ApiSecret;
        public string SessionKey { get; set; }

        private LastFmApiClient client;

        public KoScrobbler(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public string GetMobileSession(string userName, string password)
        {
            var method = "auth.getmobilesession";

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            var response = GetClient().Post<GetSessionResponse>(method, parameters);;
            
            return response.Session.Key;
        }

        public void TryScrobble(List<Scrobble> scrobbles)
        {
            var method = "track.scrobble";

            if (string.IsNullOrEmpty(SessionKey))
                throw new SessionKeyInvalidException();          
            
            var parameters = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < scrobbles.Count; i++)
            {
                var unixTimestamp = (int)(scrobbles[i].TimePlayed.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                parameters.Add(new KeyValuePair<string, string>($"track[{i}]", scrobbles[i].Title));
                parameters.Add(new KeyValuePair<string, string>($"artist[{i}]", scrobbles[i].Artist));
                parameters.Add(new KeyValuePair<string, string>($"album[{i}]", scrobbles[i].Album));
                parameters.Add(new KeyValuePair<string, string>($"timestamp[{i}]", unixTimestamp.ToString()));
                parameters.Add(new KeyValuePair<string, string>($"chosenByUser[{i}]", "0"));
            }
            
            var httpResponse = GetClient().Post<object>(method, parameters);
        }

        private LastFmApiClient GetClient()
        {
            if (client == null)
                client = new LastFmApiClient();
            client.SessionKey = SessionKey;
            return client;
        }
    }
}
