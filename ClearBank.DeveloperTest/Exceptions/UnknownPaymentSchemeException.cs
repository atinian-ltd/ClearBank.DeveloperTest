using System;
using System.Runtime.Serialization;

namespace ClearBank.DeveloperTest.Exceptions
{
    [Serializable]
    internal class UnknownPaymentSchemeException : Exception
    {
        public UnknownPaymentSchemeException()
        {
        }

        public UnknownPaymentSchemeException(string message) : base(message)
        {
        }

        public UnknownPaymentSchemeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownPaymentSchemeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}