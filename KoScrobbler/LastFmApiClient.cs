using KoScrobbler.Entities.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KoScrobbler
{
    internal class LastFmApiClient
    {
        private readonly HttpClient Client;
        internal string SessionKey { get; set; }

        internal LastFmApiClient()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://ws.audioscrobbler.com/2.0/");
        }

        internal async Task<T> Post<T>(string method, List<KeyValuePair<string, string>> parameters)
        {
            var content = GetContent(method, parameters);

            var result = await Client.PostAsync(string.Empty, content);

            return GetResponse<T>(result);
        }

        internal async Task<T> Get<T>(string method, List<KeyValuePair<string, string>> parameters)
        {
            var query = CreateGetQuery(method, parameters);

            var result = await Client.GetAsync(query);

            return GetResponse<T>(result);
        }

        private T GetResponse<T>(HttpResponseMessage result)
        {
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new WebRequestException(result);

            var responseString = result.Content?.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }

        private string CreateGetQuery(string method, List<KeyValuePair<string, string>> parameters)
        {
            var queryParams = new List<KeyValuePair<string, string>>
            {
                CreateParam("method", method),
                CreateParam("api_key", Scrobbler.ApiKey),
                CreateParam("format", "json")
            };

            queryParams.AddRange(parameters);

            var url = "?";

            foreach (var parameter in queryParams)
                url += $"{parameter.Key}={parameter.Value}&";

            return url.TrimEnd('&');
        }

        private FormUrlEncodedContent GetContent(string method, List<KeyValuePair<string, string>> parameters)
        {
            var contentParameters = GetSignatureParams(method, parameters);

            var signature = CreateSignature(contentParameters);

            contentParameters.Add(CreateParam("format", "json"));
            contentParameters.Add(CreateParam("api_sig", signature));

            return new FormUrlEncodedContent(contentParameters);
        }

        private List<KeyValuePair<string, string>> GetSignatureParams(string method, List<KeyValuePair<string, string>> parameters)
        {
            var contentParameters = new List<KeyValuePair<string, string>>
            {
                CreateParam("api_key", Scrobbler.ApiKey),
                CreateParam("method", method)
            };

            if (!string.IsNullOrEmpty(SessionKey))
                contentParameters.Add(CreateParam("sk", SessionKey));

            contentParameters.AddRange(parameters);
            
            return contentParameters;
        }

        private static string CreateSignature(List<KeyValuePair<string, string>> parameters)
        {
            parameters.Sort((s1, s2) => s1.Key.CompareTo(s2.Key));

            var hashString = string.Empty;
            foreach (var parameter in parameters)
                hashString += parameter.Key + parameter.Value;
            hashString += Scrobbler.ApiSecret;

            var signature = GetHash(hashString);

            return signature;
        }

        private static string GetHash(string input)
        {
            var byteString = Encoding.UTF8.GetBytes(input);

            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(byteString);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        private static KeyValuePair<string, string> CreateParam(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
