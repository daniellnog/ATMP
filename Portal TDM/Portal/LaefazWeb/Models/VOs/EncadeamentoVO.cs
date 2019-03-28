using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class EncadeamentoVO
    {
        public int? IdEncadeamento { get; set; }
        public string Descricao { get; set; }
        public int qtdTds { get; set; }
        public List<TestDataEncadeamento> testDatas { get; set; }
        public int? QtdCadastrada { get; set; }
        public int? QtdEmGeracao { get; set; }
        public int? QtdDisponivel { get; set; }
        public int? QtdErro { get; set; }
        public int? QtdReservada { get; set; }
        public int? QtdUtilizada { get; set; }
        public string GeradoPor { get; set; }
        public int TotalCount { get; set; }
        public EncadeamentoVO() {
        }

        public EncadeamentoVO(EncadeamentoVO encadeamento)
        {
         
        }

        public class TestDataEncadeamento
        {
            public string Descricao { get; set; }
            public int Id { get; set; }
            public int Ordem { get; set; }
            public string DescricaoAut { get; set; }
            public int IdTestData { get; set; }
            public List<ParametroEncadeamento> parametros { get; set; }
            public int IdAmbienteExecucao { get; set; }

            public class ParametroEncadeamento
            {
                public string Descricao { get; set; }
                public string DescricaoParametro { get; set; }
                public string DescricaoTestData { get; set; }
                public string DescricaoTipoParametro { get; set; }
                public int? IdParametroScript { get; set; }
                public int? IdParametroValor { get; set; }
                public int? IdParametroValor_Origem { get; set; }
                public int? IdTestData { get; set; }
                public int? IdTipoParametro { get; set; }
                public bool Obrigatorio { get; set; }
                public int? IdParametro { get; set; }
                public string Tipo { get; set; }
                public string Valor { get; set; }
            }
        }
    }
}