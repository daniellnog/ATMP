using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class ParametroValorVO
    {
        public string Valor { get; set; }
        public int Id { get; set; }
        public int IdParametroScript { get; set; }
        public int IdParametroValor { get; set; }
        public int IdTipoParametro { get; set; }
        public int IdParametro { get; set; }
        public int IdTestData { get; set; }
        public int IdStatusTestData { get; set; }
        public int? IdParametroValor_Origem { get; set; }
        public Dictionary<string, int?> DescricaoOrigem { get; set; }
        public string DescricaoParametro { get; set; }
        public string DescricaoTipoParametro { get; set; }
        public string DescricaoTestData { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public bool Obrigatorio { get; set; }
        public int TotalCount { get; set; }

        public ParametroValorVO()
        {
            DescricaoOrigem = this.DescricaoOrigem;
        }
    }
}