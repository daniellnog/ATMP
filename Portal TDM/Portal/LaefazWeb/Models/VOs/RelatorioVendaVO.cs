using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;

namespace TDMWeb.Models.VOs
{
    public class RelatorioVendaVO
    {
        [Display(Name = "SEF/NFE/ECF")]
        public string SEF_NFE_ECF { get; set; }

        [Display(Name = "Número NF/COO")]
        public string NUM_NF { get; set; }

        [Display(Name = "Data de Emissão")]
        public string DataEmissao { get; set; }

        //[Display(Name = "Dt. Entrada")]
        //public string DataEntrada { get; set; }
        
        [Display(Name = "Descrição Original")]
        public string Descricao { get; set; }

        [Display(Name = "Descrição Unificada")]
        public string DescricaoUnificada { get; set; }

        [Display(Name = "Quantidade")]
        public decimal? Quantidade { get; set; }

        [Display(Name = "Unidade")]
        public string DescricaoUnidade { get; set; }

        [Display(Name = "Valor Unitário")]
        public decimal? ValorUnitario { get; set; }

        [Display(Name = "Valor Total")]
        public decimal? ValorTotal { get; set; }
        
        [Display(Name = "CFOP Saída")]
        public int? CFOP { get; set; }

        [Display(Name = "SEF/NFE")]
        public string SEF_NFE { get; set; }

        [Display(Name = "NF Entrada")]
        public string NFEntrada { get; set; }
        
        [Display(Name = "Data de Entrada")]
        public string DataEntradaMenor { get; set; }

        [Display(Name = "CFOP Entrada")]
        public int? CFOPEntrada { get; set; }

        [Display(Name = "Vlr Unit Mínimo")]
        public decimal? ValorUnitarioMenor { get; set; }
               
        [Display(Name = "Diferença Valor Unit")]
        public decimal? ValorUnitarioDiferenca { get; set; }
        
        [Display(Name = "BC a Estornar")]
        public decimal? BCEstorno { get; set; }
        
        [Display(Name = "ICMS a Estornar")]
        public decimal? ICMSEstornar { get; set; }

        public int TotalCount { get; set; }

        public RelatorioVendaVO() { }
    }
}