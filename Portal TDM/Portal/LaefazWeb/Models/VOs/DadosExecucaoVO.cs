using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaefazWeb.Models.VOs
{
    public class DadosExecucaoVO
    {
        public int? IdTestData { get; set; }
        public int? IdExecucao { get; set; }
        public string DatapoolName { get; set; }
        public string DemandaName { get; set; }
        public string AmbienteVirtual { get; set; }
       
        public DadosExecucaoVO() {}
        public DadosExecucaoVO(DadosExecucaoVO dadosExecucao){}
    }
}