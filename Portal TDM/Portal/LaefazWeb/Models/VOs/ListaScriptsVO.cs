using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class ListaScriptsVO
    {
        public int IdPlanoTeste_TI { get; set; }
        public int IdScript_CondicaoScript { get; set; }
        public string DescricaoScript { get; set; }
        public string DescricaoPlataforma { get; set; }
    }
}