using System;
using System.Net.Http;

namespace KoScrobbler.Entities.Exceptions
{
    internal class WebRequestException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; private set; }

        internal WebRequestException(HttpResponseMessage message)
        {
            ResponseMessage = message;
        }
    }
}
