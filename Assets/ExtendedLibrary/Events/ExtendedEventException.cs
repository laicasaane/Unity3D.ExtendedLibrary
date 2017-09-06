using System;

namespace ExtendedLibrary.Events
{
    public class ExtendedEventException : Exception
    {
        public ExtendedEventException()
        {
        }

        public ExtendedEventException(string message) : base(message)
        {
        }

        public ExtendedEventException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
