using Newtonsoft.Json;
using System.Collections.Generic;

namespace KoScrobbler.Entities.LastFmApi
{
    internal class LastFmScrobbleResponse
    {
        [JsonProperty("scrobbles")]
        public Scrobbles Scrobbles { get; set; }
    }

    internal class Scrobbles
    {
        [JsonProperty("@attr")]
        Attributes Attributes { get; set; }
        [JsonProperty("scrobble")]
        List<ResponseScrobble> ScrobbleList { get; set; }
    }

    internal class Attributes
    {
        [JsonProperty("accepted")]
        public int Accepted { get; set; }
        [JsonProperty("ignored")]
        public int Ignored { get; set; }
    }

    internal class ResponseScrobble
    {
        [JsonProperty("artist")]
        public LastFmReturnProperty Artist { get; set; }
        [JsonProperty("ignoredMessage")]
        public IgnoredMessage IgnoredMessage { get; set; }
        [JsonProperty("albumArtist")]
        public LastFmReturnProperty AlbumArtist { get; set; }
        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }
        [JsonProperty("album")]
        public LastFmReturnProperty Album { get; set; }
        [JsonProperty("track")]
        public LastFmReturnProperty Track { get; set; }
    }

    internal class LastFmReturnProperty
    {
        [JsonProperty("corrected")]
        public int Corrected { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    internal class IgnoredMessage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }
}
