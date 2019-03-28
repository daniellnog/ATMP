using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaefazWeb.Enumerators
{
    public enum EnumStatusExecucao
    {
        AguardandoProcessamento = 1,
        EmProcessamento = 2,
        ProcessandoLogTosca = 3,
        Sucesso = 4,
        Falha = 5,
        Cancelada = 6,
        EmCancelamento = 7,
        Agendada = 8

        
    }
}