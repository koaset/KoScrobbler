using KoScrobbler.Interfaces;
using System;
using System.Collections.Generic;
using KoScrobbler.Entities;
using KoScrobbler.Entities.Exceptions;
using KoScrobbler.Entities.LastFmApi;

namespace KoScrobbler
{
    public class Scrobbler : ILastFmScrobbler
    {
        internal static string ApiKey;
        internal static string ApiSecret;
        private LastFmApiClient client;
        public string SessionKey { get; set; }

        private LastFmApiClient Client
        {
            get
            {
                if (client == null)
                    client = new LastFmApiClient();
                client.SessionKey = SessionKey;
                return client;
            }
        }

        public Scrobbler(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public GetSessionResult CreateSession(string userName, string password)
        {
            var method = "auth.getmobilesession";

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            var response = TryPost<LastFmGetSessionResponse>(method, parameters);

            if (response?.Session == null)
                return new GetSessionResult();

            return new GetSessionResult()
            {
                SessionKey = response.Session.Key,
                Success = true
            };
        }

        public ValidateSessionResult ValidateSession(string userName, string sessionKey)
        {
            var method = "user.getInfo";

            ValidateSessionKey(sessionKey);

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("sk", sessionKey)
            };

            var getInfoResponse = TryGet<LastFmGetUserInfoResponse>(method, parameters);

            if (getInfoResponse?.UserInfo == null || 
                !string.Equals(getInfoResponse.UserInfo.Name, userName, StringComparison.InvariantCulture))
                return new ValidateSessionResult();

            SessionKey = sessionKey;

            return new ValidateSessionResult
            {
                Success = true,
                UserName = userName,
                SessionKey = sessionKey
            };
        }

        public ScrobbleResult TryScrobble(List<Scrobble> scrobbles)
        {
            var method = "track.scrobble";

            ValidateSessionKey(SessionKey);

            var parameters = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < scrobbles.Count; i++)
            {
                parameters.Add(new KeyValuePair<string, string>($"track[{i}]", scrobbles[i].Title));
                parameters.Add(new KeyValuePair<string, string>($"artist[{i}]", scrobbles[i].Artist));
                parameters.Add(new KeyValuePair<string, string>($"album[{i}]", scrobbles[i].Album));
                parameters.Add(new KeyValuePair<string, string>($"timestamp[{i}]", UnixTimeStamp(scrobbles[i].TimePlayed)));
                parameters.Add(new KeyValuePair<string, string>($"chosenByUser[{i}]", "0"));
            }

            var response = TryPost<LastFmScrobbleResponse>(method, parameters);

            if (response.Scrobbles == null)
                return new ScrobbleResult();

            return new ScrobbleResult
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
                return Client.Post<T>(method, parameters);
            }
            catch (WebRequestException)
            {
                return new T();
            }
        }

        private T TryGet<T>(string method, List<KeyValuePair<string, string>> parameters) where T : new()
        {
            try
            {
                return Client.Get<T>(method, parameters);
            }
            catch (WebRequestException)
            {
                return new T();
            }
        }

        private static void ValidateSessionKey(string sessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
                throw new SessionKeyInvalidException();
        }

        private static string UnixTimeStamp(DateTime timePlayed)
        {
            var utcTime = timePlayed.ToUniversalTime();
            var unixTimeStamp = (int)(utcTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimeStamp.ToString();
        } 
    }
}
