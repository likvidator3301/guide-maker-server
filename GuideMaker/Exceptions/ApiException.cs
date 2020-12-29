using System;
using System.Net;

namespace GuideMaker.Exceptions
{
    internal class ApiException: Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
