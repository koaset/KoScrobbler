using System;
using System.Net.Http;
using System.Xml;

namespace KoScrobbler.Entities
{
    public class GetTokenResponse : LastFmResponseBase
    {
        public string SessionToken { get; set; }

        public GetTokenResponse(HttpResponseMessage response)
        {
            if (response?.Content == null)
                throw new ArgumentNullException();

            Message = response.Content.ReadAsStringAsync().Result;

            var xml = new XmlDocument();
            xml.LoadXml(Message);
            SessionToken = xml["lfm"]["token"].InnerText;

            Successful = !string.IsNullOrEmpty(SessionToken);
        }
    }
}
