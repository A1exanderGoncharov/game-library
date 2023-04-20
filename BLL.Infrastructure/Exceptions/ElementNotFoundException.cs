using System;

namespace BLL.Infrastructure.Exceptions
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException() { }

        public ElementNotFoundException(string message)
            : base(message) { }

        public ElementNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        public ElementNotFoundException(string elementName, int elementId)
            : base($"The element '{elementName}' with id '{elementId}' was not found.")
        {
        }
    }
}
