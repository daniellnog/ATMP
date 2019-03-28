using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class TestDataVO
    {
      
        public string Descricao { get; set; }
        public bool Selected { get; set; }
        public string CasoTesteRelativo { get; set; }
        public string Observacao { get; set; }
        public System.DateTime? DataGeracao { get; set; }
        public string GeradoPor { get; set; }
        public string Status { get; set; }
        public List<ParametroValorVO> valores { get; set; }
        public List<AmbienteExecucaoVO> AmbientesExecs { get; set; }
        public int? IdTestData { get; set; }
        public int? IdParametroValor { get; set; }
        public int? IdStatus { get; set; }
        public int? IdScriptCondicaoScript { get; set; }
        public int IdDataPool { get; set; }
        public int OrdemEncadeamento { get; set; }
        public string migracaoCodigo { get; set; }
        public string CaminhoEvidencia { get; set; }
        public System.DateTime? TempoEstimadoExecucao { get; set; }
        public string tempoExecString { get; set; }
        public int? idStatusExec { get; set; }

        public int Ordem { get; set; }

        public int? ClassificacaoMassa { get; set; }

        public int? IdAut { get; set; }
        public string DescricaoAut { get; set; }
        public string DescricaoStatus { get; set; }
        public string DescricaoDataPool{ get; set; }

    }
}