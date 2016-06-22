using Newtonsoft.Json;

namespace KoScrobbler.Entities.LastFmApi
{
    internal class LastFmGetUserInfoResponse
    {
        [JsonProperty("user")]
        public User UserInfo { get; set; }
    }

    internal class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("age")]
        public string Age { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("playcount")]
        public string Playcount { get; set; }
    }
}
