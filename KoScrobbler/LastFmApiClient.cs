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

        internal T Post<T>(string method, List<KeyValuePair<string, string>> parameters)
        {
            var content = GetContent(method, parameters);

            var request = Client.PostAsync(string.Empty, content);
            
            MakeRequest(request);

            return GetResponse<T>(request.Result);
        }

        internal T Get<T>(string method, List<KeyValuePair<string, string>> parameters)
        {
            var query = CreateGetQuery(method, parameters);
            var request = Client.GetAsync(query);

            MakeRequest(request);

            return GetResponse<T>(request.Result);
        }

        private T GetResponse<T>(HttpResponseMessage result)
        {
            var responseString = result.Content?.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }

        private void MakeRequest(Task<HttpResponseMessage> request)
        {
            request.Wait();

            if (request.Result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new WebRequestException(request.Result);
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

        private static string ParametersToString(List<KeyValuePair<string, string>> parameters)
        {
            var returnString = string.Empty;
            foreach (var param in parameters)
                returnString += $"&{param.Key}={param.Value}";
            return returnString;
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
