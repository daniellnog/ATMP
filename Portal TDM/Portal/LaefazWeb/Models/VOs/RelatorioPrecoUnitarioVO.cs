using System.ComponentModel.DataAnnotations;

namespace TDMWeb.Models.VOs
{
    public class RelatorioPrecoUnitarioVO
    {

        [Display(Name = "Descrição Unificada")]
        public string Descricao { get; set; }

        [Display(Name = "Valor Unitário")]
        public decimal? ValorUnitario { get; set; }

        [Display(Name = "Origem")]
        public string PlanilhaOrigem { get; set; }

        [Display(Name = "Num. NF")]
        public int? NUM_NF { get; set; }

        [Display(Name = "Data de Emissão")]
        public string DataEntrada { get; set; }

        public int TotalCount { get; set; }
    }
}