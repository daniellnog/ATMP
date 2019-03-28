namespace LaefazWeb.Models.VOs
{
    public class ParametroVO
    {
        public string Descricao { get; set; }
        public int? IdParametroScript { get; set; }
        public int IdParametro { get; set; }
        public int IdTipoParametro { get; set; }
        public string Tipo { get; set; }
        public bool Obrigatorio { get; set; }
        public int IdScript_CondicaoScript { get; set; }
        public string ColunaTecnicaTosca { get; set; }
        public string ValorParametroDefault { get; set; }
        public bool VisivelEmTela { get; set; }
        public string ValorDefault { get; set; }

        public ParametroVO(int IdParametro, string Descricao, string Tipo, string ValorDefault, bool Obrigatorio, bool VisivelEmTela)
        {
            this.IdParametro = IdParametro;
            this.Descricao = Descricao;
            this.Tipo = Tipo;
            this.ValorDefault = ValorDefault;
            this.Obrigatorio = Obrigatorio;
            this.VisivelEmTela = VisivelEmTela;
        }

        public ParametroVO()
        {
                
        }
    }
}