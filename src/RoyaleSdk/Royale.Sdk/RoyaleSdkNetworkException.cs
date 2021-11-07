using System;
using System.Net;

namespace Royale.Sdk
{
    public class RoyaleSdkNetworkException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public RoyaleSdkNetworkException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
