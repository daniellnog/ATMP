namespace LaefazWeb.Models.VOs
{
    public class ExecucaoVO
    {
        public int? Id { get; set; }
        public int? IdScript_CondicaoScript_Ambiente { get; set; }
        public int? IdTestData { get; set; }
        public string DataAgendamento { get; set; }
        public string DescricaoATMP { get; set; }
        public string DescricaoDatapool { get; set; }
        public string DescricaoTestData { get; set; }
        public int? IdStatusExecucao { get; set; }
        public int? IdAmbienteVirtual { get; set; }
        public string AmbienteVirtual { get; set; }
        public string CaminhoArquivoTCS { get; set; }
        public string DiretorioRelatorio { get; set; }
        public int? IdEncadeamento { get; set; }
        public bool Encadeado { get; set; }
        public bool EnvioTelegram { get; set; }
        public string ListaExecucaoTosca { get; set; }
        public string NomeQueryTosca { get; set; }
        public string QueryTosca { get; set; }

        public ExecucaoVO() {

          // DbEntities db = new DbEntities();
          //  this.Encadeado = this.IdEncadeamento.Equals(null) ? false : true;

        }

        public ExecucaoVO(ExecucaoVO execucao)
        {
         
        }
    }
}