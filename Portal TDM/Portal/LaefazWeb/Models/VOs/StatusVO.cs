using LaefazWeb.Models;
using System;

namespace TDMWeb.Models.VOs
{
    public class StatusVO
    {
        public Status Status { get; set; }
        public StatusExecucao StatusExecucao { get; set; }
        public String Tipo { get; set; }
        public int Id { get; set; }
        public String Descricao { get; set; }

    }
}