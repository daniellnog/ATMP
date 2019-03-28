using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaefazWeb.Models.VOs
{
    public class Script_CondicaoScript_CT_VO
    {
        public int Id { get; set; }
        public int IdScript_CondicaoScript { get; set; }
        public int IdCT { get; set; }
        public int IdAmbienteExecucao { get; set; }
        public int? IdStatusUltimaExecucao { get; set; }
		public int IdAut { get; set; }
        public string DescricaoAmbienteExecucao { get; set; }
        public string FaseCT { get; set; }
        public string NomeTecnicoScript { get; set; }
        public string DescricaoScript { get; set; }
        public string DescricaoPlataforma { get; set; }
        public string DescricaoFase { get; set; }
        public string DescricaoSistema { get; set; }
        public string DescricaoCT { get; set; }
        public string DescricaoStatusUltimaExecucao { get; set; }
		public string DescricaoAut { get; set; }
    }
}