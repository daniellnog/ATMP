namespace LaefazWeb.Models.VOs
{
    public class TestDataCtVO
    {
        public int Id { get; set; }
        public int IdTestData { get; set; }
        public int IdStatusTestData { get; set; }
        public string CaminhoEvidencia { get; set; }
        public string MassaDeDados { get; set; }
        public string Demanda { get; set; }
        public string NumeroCasoDeTeste { get; set; }
        public string Status { get; set; }
        public string GeradoPor { get; set; }
        public System.DateTime? DataGeracao { get; set; }
        public string DescricaoScript { get; set; }
        public string DescricaoPlataforma { get; set; }

        public TestDataCtVO() {

        }

    }
}