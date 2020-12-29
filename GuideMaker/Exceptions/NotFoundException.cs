using System.Net;

namespace GuideMaker.Exceptions
{
    internal sealed class NotFoundException: ApiException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
