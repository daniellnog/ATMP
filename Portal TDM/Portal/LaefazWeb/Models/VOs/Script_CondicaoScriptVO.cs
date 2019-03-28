using LaefazWeb.Models;
using System;
using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class Script_CondicaoScriptVO
    {
        public int Id { get; set; }
        public int IdScript { get; set; }
        public int IdAut { get; set; }
        public string DescricaoScript { get; set; }
        public string DescricaoCondicaoScript { get; set; }
        public Nullable<int> IdCondicaoScript { get; set; }
        public string QueryTosca { get; set; }
        public string ListaExecucaoTosca { get; set; }
        public string CaminhoArquivoTCS { get; set; }
        public string DiretorioRelatorio { get; set; }
        public Nullable<System.DateTime> TempoEstimadoExecucao { get; set; }
    }
}