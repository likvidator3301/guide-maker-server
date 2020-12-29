using System.Net;

namespace GuideMaker.Exceptions
{
    internal sealed class ConflictException: ApiException
    {
        public ConflictException(string message) : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
