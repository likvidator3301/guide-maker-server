using System.Net;

namespace GuideMaker.Exceptions
{
    internal sealed class UnauthorizedException: ApiException
    {
        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message)
        { }
    }
}
