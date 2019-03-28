using System.ComponentModel.DataAnnotations;

namespace TDMWeb.Models.VOs
{
    public class RelatorioLevantamentoVO
    {
        [Display(Name = "Descrição Unificada")]
        public string ProdutoDescricao { get; set; }

        [Display(Name = "Unidade")]
        public string UnidadeDescricao { get; set; }
        
        [Display(Name = "Estoque Inicial SEF")]
        public decimal? QtdInicial { get; set; }
        
        [Display(Name = "Entradas")]
        public decimal? QtdEntrada { get; set; }
        
        [Display(Name = "Saídas")]
        public decimal? QtdSaida { get; set; }
        
        [Display(Name = "Estoque Final Calculado")]
        public decimal? QtdFinalCalculado { get; set; }

        [Display(Name = "Estoque Final SEF")]
        public decimal? QtdFinal { get; set; }

        [Display(Name = "Diferença")]
        public decimal? QtdDiferenca { get; set; }

        [Display(Name = "Omissão")]
        public string OMISSAO { get; set; }

        [Display(Name = "Valor Unitário (R$)")]
        public decimal? ValorUnitario { get; set; }

        [Display(Name = "Base Cálculo (R$)")]
        public decimal BaseCalculo { get; set; }
        
        public int TotalCount { get; set; }
        public int? IdProdutoPai { get; set; }
        public string CorLinha { get; set; }
        public RelatorioLevantamentoVO() { }
    }
}