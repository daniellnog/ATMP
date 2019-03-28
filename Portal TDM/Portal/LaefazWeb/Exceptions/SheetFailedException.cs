using System;

namespace TDMWeb.Exceptions
{
    public class SheetFailedException : Exception
    {
        public SheetFailedException()
        { }
        public SheetFailedException(string message)
            : base(message)
        { }
    }
}