using System.Net;

namespace GuideMaker.Exceptions
{
    internal sealed class ForbiddenException: ApiException
    {
        public ForbiddenException(string message) : base(HttpStatusCode.Forbidden, message)
        {
        }
    }
}
