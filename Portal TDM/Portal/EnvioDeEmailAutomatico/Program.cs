using LaefazWeb.Models;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using TDMWeb.Models.VOs;

namespace EnvioDeEmailAutomatico
{
    class Program
    {
        private static string caminhoArquivo = ConfigurationSettings.AppSettings["caminhoDestinoArquivo"];
        static void Main(string[] args)
        {
            DbEntities db = new DbEntities();

            List<EmailVO> regs = new List<EmailVO>();

            regs = db.Database.SqlQuery<EmailVO>("EXEC PR_LISTAR_DATAPOOL_ENVIO_EMAIL").ToList();

            StreamWriter sw = new StreamWriter(@caminhoArquivo);
            //sw.WriteLine("");
            //sw.WriteLine("<style>.td { text-align: center; } .titulo { white-space: nowrap; font-weight: bold } </style>");
            sw.WriteLine("<html><head><meta charset='UTF-8'><title></title></head><body><style>body {  font-size: 8px 'Myriad Pro', Frutiger, 'Lucida Grande', 'Lucida Sans', 'Lucida Sans Unicode', Verdana, sans-serif;}table {  border-collapse: collapse;  width: 50em;  border: 1px solid #666;}thead {  background: #ccc left center;  border-top: 1px solid #a5a5a5; border-bottom: 1px solid #a5a5a5;} thead tr:hover { background-color: transparent;  color: inherit;} tr:nth-child(even) {    background-color: #B0C4DE;} th { font-weight: normal;  text-align: left;}th, td {  padding: 0.1em 1em; text-align: center;} .titulo { white-space: normal; font-weight: bold }</style><table border='2' cellpadding='2' cellspacing='2' style='width: 70%'><tbody>");
            sw.WriteLine("<thead><tr><td class='titulo'>Descrição do Datapool</td><td class='titulo'>Data Término</td><td class='titulo'>Descrição da Demanda</td><td class='titulo'>Descrição do Sistema</td><td class='titulo'>Qtd. Cadastrada</td><td class='titulo'>Qtd.Solicitada</td><td class='titulo'>Qtd. Disponível</td><td class='titulo'>Qtd. Reservada</td><td class='titulo'>Qtd. Utilizada</td><td class='titulo'>Farol</td><td class='titulo'>Data de Solicitação</td><td class='titulo'>Descrição do TDM</td><td class='titulo'>Descrição do Script/Condição do Script</td></tr></thead>");

            foreach (EmailVO EmailVO in regs)
            {
                // nova linha
                string linhaInicio = "<tr>";
                //colunas
                string colunaDatapool = "<td>" + EmailVO.DataPool + "</td>";
                string colunaDataTermino = "<td>" + EmailVO.DataTermino + "</td>";
                string colunaDescricaoDemanda = "<td>" + EmailVO.DescricaoDemanda + "</td>";
                string colunaDescricaoSistema = "<td>" + EmailVO.DescricaoSistema + "</td>";
                string colunaQtdCadastrada = "<td>" + EmailVO.QtdCadastrada + "</td>";
                string colunaQtdSolicitada = "<td>" + EmailVO.QtdSolicitada + "</td>";
                string colunaQtdDisponivel = "<td>" + EmailVO.QtdDisponivel + "</td>";
                string colunaQtdReservada = "<td>" + EmailVO.QtdReservada + "</td>";
                string colunaQtdUtilizada = "<td>" + EmailVO.QtdUtilizada + "</td>";
                string colunaFarol = "<td>" + EmailVO.Farol + "</td>";
                string colunaDataSolicitacao = "<td>" + EmailVO.DataSolicitacao + "</td>";
                string colunaDescricaoTDM = "<td>" + EmailVO.DescricaoTDM + "</td>";
                string colunaDescricaoCondicaoScript = "<td>" + EmailVO.DescricaoCondicaoScript + "</td>";
                //fechando linha
                string linhaFim = "</tr>";

                string conteudo = linhaInicio + colunaDatapool + colunaDataTermino + colunaDescricaoDemanda + colunaDescricaoSistema + colunaQtdCadastrada
                    + colunaQtdSolicitada + colunaQtdDisponivel + colunaQtdReservada + colunaQtdUtilizada + colunaFarol + colunaDataSolicitacao + 
                    colunaDescricaoTDM + colunaDescricaoCondicaoScript + linhaFim;

                sw.WriteLine(conteudo);
            }
            sw.WriteLine("</tbody></table></body></html>");
            sw.Close();
        }
    }
}
