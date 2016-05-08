using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KoScrobbler
{
    internal class LastFmApiClient
    {
        private readonly HttpClient Client;
        internal Dictionary<string, string> AdditionalParameters { get; set; }
        internal string Method { get; set; }

        internal string SessionKey { get; set; }

        internal LastFmApiClient(string method, Dictionary<string, string> queryParameters) : base()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://ws.audioscrobbler.com/2.0/");

            Method = method;
            AdditionalParameters = queryParameters;
        }

        internal HttpResponseMessage Get()
        {
            var queryString = CreateQueryString();
            var response = Client.GetAsync(queryString);
            return response.Result;
        }

        private string CreateQueryString()
        {
            return $"?method={Method}" +
                $"&api_key={KoScrobbler.ApiKey}" +
                $"&api_sig={CreateApiSignatureString(Method, AdditionalParameters)}" +
                ParameterDictionaryToString(AdditionalParameters) +
                (string.IsNullOrEmpty(SessionKey) ? string.Empty : $"&sk={SessionKey}");
        }

        private static string ParameterDictionaryToString(Dictionary<string, string> parameterDictionary)
        {
            var returnString = string.Empty;
            foreach (var param in parameterDictionary.Keys)
                returnString += $"&{param}={parameterDictionary[param]}";
            return returnString;
        }

        // method=auth.gettoken&username=koaset_test&password=!bef2hiTutm&api_key=4eedd270b2824403af15f9a81407f7ce&api_sig=koplayer

        private static string GetConcatenatedParameterString(string method, Dictionary<string, string> additionalParameters)
        {
            var parameterDictionary = CreateCompleteParameterDictionary(method, additionalParameters);
            
            var keyList = parameterDictionary.Keys.ToList();
            keyList.Sort();

            var returnString = string.Empty;

            foreach (var param in keyList)
                returnString += $"{param}{parameterDictionary[param]}";

            return returnString;
        }

        private static Dictionary<string, string> CreateCompleteParameterDictionary(string method, Dictionary<string, string> additionalParameters)
        {
            var parameterDictionary = new Dictionary<string, string>
            {
                { "method", method},
                { "api_key", KoScrobbler.ApiKey}
            };

            foreach (var param in additionalParameters.Keys)
                parameterDictionary.Add(param, additionalParameters[param]);

            return parameterDictionary;
        }

        private static string CreateApiSignatureString(string method, Dictionary<string, string> additionalParameters)
        {
            var hashString = GetConcatenatedParameterString(method, additionalParameters);

            var byteString = Encoding.UTF8.GetBytes(hashString);

            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(byteString);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
