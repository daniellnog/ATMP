using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TDMWeb.Models.VOs
{
    public class PlanilhaMovimentacaoEntradaVO
    {
        [Display(Name = "SEF / NFE")]
        public string SEF_NFE_ECF { get; set; }

        [Display(Name = "Num. NF")]
        public string NUM_NF { get; set; }

        [Display(Name = "Data de Entrada")]
        public string DataEntrada { get; set; }

        [Display(Name = "Descrição Original")]
        public string ProdutoDescricao { get; set; }

        [Display(Name = "Descrição Unificada")]
        public string ProdutoUnificado { get; set; }

        [Display(Name = "Quantidade")]
        public decimal? Quantidade { get; set; }

        [Display(Name = "Unidade")]
        public string UnidadeDescricao { get; set; }       

        [Display(Name = "Valor Unitário")]
        public decimal? ValorUnitario { get; set; }

        [Display(Name = "Valor Total")]
        public decimal? ValorTotal { get; set; }

        [Display(Name = "CFOP")]
        public int CFOP { get; set; }

        public int TotalCount { get; set; }

        public PlanilhaMovimentacaoEntradaVO() { }
    }
}