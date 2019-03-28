namespace TDMWeb.ViewModel
{
    public class ProdutoExcel
    {
        public int Id { get; set; }
        public string DescricaoOriginalProduto { get; set; }
        public string DescricaoUnificada { get; set; }
        public string Unidade { get; set; }
        public System.Nullable<bool> JaUnificado { get; set; }

        public override string ToString()
        {
            return string.Concat(Id, ";", DescricaoOriginalProduto, ";", DescricaoUnificada, ";", Unidade);
        }
    }
}