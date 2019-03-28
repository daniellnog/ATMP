using System.ComponentModel.DataAnnotations;
namespace TDMWeb.Models.VOs
{
    public class PlanilhaProdutoVO
    {
        [Display(Name = "Descrição Original")]
        public string DescricaoOriginal { get; set; }

        [Display(Name = "Descrição Unificada")]
        public string DescricaoUnificada { get; set; }

        [Display(Name = "Unidade")]
        public string DescricaoUnidade { get; set; }

        public int TotalCount { get; set; }
        
        public PlanilhaProdutoVO() { }
    }
}