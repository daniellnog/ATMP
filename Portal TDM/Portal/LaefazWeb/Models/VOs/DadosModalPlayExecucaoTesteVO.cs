using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class DadosModalPlayExecucaoTesteVO
    {

        public List<listaVO> ListaScripts { get; set; }
        public List<listaVO> ListaTipoFaseTeste { get; set; }
        public List<listaVO> ListaTestDatas { get; set; }
    }
}