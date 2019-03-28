using System.ComponentModel.DataAnnotations;
namespace TDMWeb.Models.VOs
{
    public class ProdutoVO
    {
        public int Id { get; set; }

        public string DescricaoOriginal { get; set; }
        public string DescricaoUnificada { get; set; }
        public int IdUnidade { get; set; }
        public string DescricaoUnidade { get; set; }
        public int TotalCount { get; set; }
        public int Ativo { get; set; }
        public bool Unificado { get; set; }

        public ProdutoVO() { }

        public ProdutoVO(Produto produto)
        {
            this.Id = produto.Id;
            this.DescricaoOriginal = produto.Descricao;
            //this.DescricaoUnificada = produto.ProdutoPai != null ? produto.ProdutoPai.Descricao : "";
            this.IdUnidade = produto.Unidade.Id;
            this.DescricaoUnidade = produto.Unidade.Descricao;
        }
    }
}