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
        public Attributes Attributes { get; set; }
    }

    internal class Attributes
    {
        [JsonProperty("accepted")]
        public int Accepted { get; set; }
        [JsonProperty("ignored")]
        public int Ignored { get; set; }
    }
}
