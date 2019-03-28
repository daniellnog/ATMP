using System;

namespace TDMWeb.Exceptions
{
    public class AnaliseDuplicatedException : Exception
    {
        public AnaliseDuplicatedException()
        { }
        public AnaliseDuplicatedException(string message)
            : base(message)
        { }
    }
}