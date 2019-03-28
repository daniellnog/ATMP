using System;
using System.Collections.Generic;
using System.Linq;

namespace LaefazWeb.Models.VOs
{
    public class DataPoolVO
    {
        public int Id { get; set; }
        public int? IdAut { get; set; }
        public int? IdTDM { get; set; }
        public int? IdDemanda { get; set; }
        public int? IdCondicaoScript { get; set; }
        public int IdScriptCondicaoScript { get; set; }
        public int? IdScript { get; set; }
        public string UsuarioCriacao { get; set; }
        public string Observacao { get; set; }
        public string DescricaoDemanda { get; set; }
        public string DescricaoSistema { get; set; }
        public string DescricaoDataPool { get; set; }
        public string DescricaoCondicaoScript { get; set; }
        public string DescricaoTDM { get; set; }
        public int? QtdSolicitada { get; set; }
        public int? QtdDisponivel { get; set; }
        public int? QtdReservada { get; set; }
        public int? QtdUtilizada { get; set; }
        public int? QtdCadastrada { get; set; }
        public int Farol { get; set; }
        public int TotalCount { get; set; }
        public bool emExecucao { get; set; }
        public bool emCancelamento { get; set; }
        public bool ConsiderarRotinaDiaria { get; set; }
        public DataPoolVO()
        {
            DbEntities db = new DbEntities();

            List<TestData> listaMassasTestes = new List<TestData>();
            listaMassasTestes.AddRange(db.TestData.Where(x => x.IdDataPool == Id).ToList());
            this.ListaMassa = listaMassasTestes;
        }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataTermino { get; set; }
        public virtual Demanda Demanda { get; set; }
        public virtual AUT AUT { get; set; }
        public List<TestData> ListaMassa { get; set; }
        public DataPoolVO(DataPool datapool)
        {
            DbEntities db = new DbEntities();

            this.Id = datapool.Id;
            this.DescricaoDataPool = datapool.Descricao;
            this.DescricaoDemanda = datapool.Demanda != null ? datapool.Demanda.Descricao : "";
            this.DescricaoSistema = datapool.AUT.Descricao != null ? datapool.AUT.Descricao : "";
            this.DataSolicitacao = datapool.DataSolicitacao;
            this.DataTermino = datapool.DataTermino;
            this.DescricaoTDM = datapool.TDM.Descricao;
            this.IdScriptCondicaoScript = datapool.IdScript_CondicaoScript;
            List<TestData> listaMassasTestes = new List<TestData>();
            listaMassasTestes.AddRange(db.TestData.Where(x => x.IdDataPool == datapool.Id).ToList());

            this.ListaMassa = listaMassasTestes;
            //DataPoolVOs.ListaMassa.ad

        }
    }
}


