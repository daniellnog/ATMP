using System;
using System.Collections.Generic;
using System.Linq;

namespace LaefazWeb.Models.VOs
{
    public class CTVO
    {
        public int IdPlanoTeste_TI { get; set; }
        //public string DescricaoProjeto { get; set; }
        //public string DescricaoSistema { get; set; }
        //public string DescricaoPasta { get; set; }
        public string Plano_Teste { get; set; }
        public bool? ExecucaoAutomatizada_ATMP { get; set; }
        public string MotivoExecucao_ATMP { get; set; }
        public string Ofensor_ATMP { get; set; }
        public decimal? NRO_CT { get; set; }
        public decimal? NRO_CENARIO { get; set; }
        public string NOME { get; set; }
        public string LISTA_SCRIPTS { get; set; }
        public List<listaVO> ListaScripts { get; set; }
        public List<MotivoExecucao> MotivosExecucoes { get; set; }
        public string SCRIPT_SELECIONADO { get; set; }
        public int TotalCount { get; set; }
    }
}