using System;

namespace TDMWeb.Models.VOs
{
    public class EmailVO
    {

        public EmailVO()
        {

        }

        public string DataPool { get; set; }
        public DateTime? DataTermino { get; set; }
        public string DescricaoDemanda { get; set; }
        public string DescricaoSistema { get; set; }
        public int QtdCadastrada { get; set; }
        public int QtdSolicitada { get; set; }
        public int QtdDisponivel { get; set; }
        public int QtdReservada { get; set; }
        public int QtdUtilizada { get; set; }
        public string Farol { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public string DescricaoTDM { get; set; }
        public string DescricaoCondicaoScript { get; set; }
    }
}