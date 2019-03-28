using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDMWeb.Models.VOs
{
    public class ContestarValorManualVO
    {
        public string Descricao { get; set; }
        public int Id { get; set; }
        public string ValorUnitario { get; set; }
        public int IdProdutoPai { get; set; }
    }
}