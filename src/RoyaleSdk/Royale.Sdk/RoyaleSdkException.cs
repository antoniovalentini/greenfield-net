using System;

namespace Royale.Sdk
{
    public class RoyaleSdkException : Exception
    {
        public RoyaleSdkException(string message) : base(message) { }
        public RoyaleSdkException(string message, Exception innerException) : base(message, innerException) { }
    }
}
