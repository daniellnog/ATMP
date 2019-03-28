using System;

namespace TDMWeb.Exceptions
{
    public class UnificacaoInvalidaException : Exception
    {
        public UnificacaoInvalidaException()
        { }

        public UnificacaoInvalidaException(string mensage)
            : base(mensage)
        { }
    }
}