namespace LaefazWeb.Models.VOs
{
    public class Alteracao
    {
        public int Chave{ get; set; }
        public int Incremento { get; set; }
        public int IdCampo { get; set; }
        public string Inicio { get; set; }
        public string Termino { get; set; }
        public string Valor { get; set; }
        public int idParamScript { get; set; }
    };    
}