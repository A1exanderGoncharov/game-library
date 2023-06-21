using System;

namespace BLL.Core.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() { }

        public ObjectNotFoundException(string message)
            : base(message) { }

        public ObjectNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        public ObjectNotFoundException(string objectName, int objectId)
            : base($"The object '{objectName}' with id '{objectId}' was not found.")
        {
        }
    }
}
