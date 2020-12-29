using System;

namespace GuideMaker.Repository.Exceptions
{
    public sealed class EntityNotFoundException: Exception
    {
        public EntityNotFoundException(string message): base(message)
        {
            
        }
    }
}
