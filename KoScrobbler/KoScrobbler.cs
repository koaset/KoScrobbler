using KoScrobbler.Interfaces;
using System;
using System.Collections.Generic;
using KoScrobbler.Entities;
using KoScrobbler.Entities.Exceptions;
using KoScrobbler.Entities.LastFmApi;

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

        public GetSessionResponse GetMobileSession(string userName, string password)
        {
            var method = "auth.getmobilesession";

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            var response = TryPost<LastFmGetSessionResponse>(method, parameters);

            if (response.Session == null)
                return new GetSessionResponse();

            return new GetSessionResponse()
            {
                SessionKey = response.Session.Key,
                Success = true
            };
        }

        public ScrobbleResponse TryScrobble(List<Scrobble> scrobbles)
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

            var response = TryPost<LastFmScrobbleResponse>(method, parameters);

            if (response == null)
                return new ScrobbleResponse();

            return new ScrobbleResponse
            {
                Success = true,
                SuccessfulScrobbles = response.Scrobbles.Attributes.Accepted,
                IgnoredScrobbles = response.Scrobbles.Attributes.Ignored
            };
        }

        private T TryPost<T>(string method, List<KeyValuePair<string, string>> parameters) where  T : new()
        {
            try
            {
                return GetClient().Post<T>(method, parameters);
            }
            catch (WebRequestException)
            {
                return new T();
            }
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
