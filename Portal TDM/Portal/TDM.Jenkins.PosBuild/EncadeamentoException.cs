using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.Jenkins.PosBuild
{
    class EncadeamentoException : Exception
    {
        public EncadeamentoException(){}

        public EncadeamentoException(string message)
        : base(message)
        {
        }

        public EncadeamentoException(string message, Exception inner)
        : base(message, inner)
        {
        }

    }
}
