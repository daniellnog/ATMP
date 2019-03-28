using System.ComponentModel.DataAnnotations;

namespace TDMWeb.Models.VOs
{
    public class RelatorioMovimentacaoVO
    {
        [Display(Name = "Descrição Original")]
        public string ProdutoDescricao { get; set; }
        [Display(Name = "Descrição Unificada")]
        public string ProdutoUnificado { get; set; }
        [Display(Name = "Unidade")]
        public string UnidadeDescricao { get; set; }
        [Display(Name = "Est. Inicial")]
        public decimal? QtdInicial { get; set; }
        [Display(Name = "Entradas")]
        public decimal? QtdEntrada { get; set; }
        [Display(Name = "Saídas")]
        public decimal? QtdSaida { get; set; }
        [Display(Name = "Est. Final")]
        public decimal? QtdFinal { get; set; }
        [Display(Name = "Vlr Unit Inicial")]
        public decimal? ValorInicial { get; set; }
        [Display(Name = "Vlr Unit Entrada")]
        public decimal? ValorEntrada { get; set; }
        [Display(Name = "Vlr Unit Saída")]
        public decimal? ValorSaida { get; set; }
        [Display(Name = "Vlr Unit Final")]
        public decimal? ValorFinal { get; set; }

        public int TotalCount { get; set; }

        public RelatorioMovimentacaoVO() { }
    }
}