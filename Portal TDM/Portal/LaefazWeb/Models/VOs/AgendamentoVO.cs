using System;
using System.Collections.Generic;

namespace TDMWeb.Models.VOs
{
    public class AgendamentoVO
    {

        public AgendamentoVO()
        {

        }
        public int? IdAgendamento { get; set; }
        public int? IdExecucao { get; set; }
        public string IdUsuario { get; set; }
        public int QtdTds { get; set; }
        public string LoginUsuario { get; set; }
        public DateTime? InicioAgendamento { get; set; }
        public DateTime? TerminoAgendamento { get; set; }
        public DateTime? TempoEstimadoExecucao { get; set; }
        public string TempoEstimadoExecucaoCalculado { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public List<int?> testDatas { get; set; }
        public string script { get; set; }
        public string condicaoScript { get; set; }
        public string sistema { get; set; }
        public bool allDay { get; set; }
        public int? idTdm { get; set; }
        public int? idAmbienteVirtual { get; set; }
        public string DescricaoAmbienteExecucao { get; set; }
        public string DescricaoAmbienteVirtual { get; set; }
        public int? idAmbienteExecucao { get; set; }
        public int? idFase { get; set; }
        public string status { get; set; }
        public int? idDatapool { get; set; }
        public bool telegram { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }
        public string Output { get; set; }
    }
}