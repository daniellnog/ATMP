using System.Collections.Generic;

namespace LaefazWeb.Models.VOs
{
    public class Encadeamento_TestDataVO
    {
      
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string AUT { get; set; }
        public Encadeamento encadeamento { get; set; }
        public List<TestDataVO> testDatas { get; set; }

        public int qtdTDs { get; set; }
        public int qtdErros { get; set; }
        public int qtdSuccess { get; set; }
        public int qtdEmGeracao { get; set; }

        public int qtdCadastrada { get; set; }

        public Encadeamento_TestDataVO()
        {

        }

    }
}