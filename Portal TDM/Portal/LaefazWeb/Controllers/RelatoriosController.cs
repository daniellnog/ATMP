using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDMWeb.Enumerators;
using TDMWeb.Extensions;
using TDMWeb.Lib;
using TDMWeb.Models;
using TDMWeb.Models.VOs;

namespace TDMWeb.Controllers
{
    [UsuarioLogado]
    public class RelatoriosController : Controller
    {
        private DbEntities db = new DbEntities();

        private const string NOME_ARQUIVO_MOVIMENTACAO = "LAEFAZ_Movimentacao.zip";
        private const string NOME_ARQUIVO_PRECOUNITARIO = "LAEFAZ_PrecoUnitario.zip";
        private const string NOME_ARQUIVO_VENDA = "LAEFAZ_VendaAbaixoDeCusto.zip";
        private const string NOME_ARQUIVO_LEVANTAMENTO = "LAEFAZ_Levantamento.zip";

        #region Relatório Movimentação

        public ActionResult Movimentacao()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarRelatorioMovimentacao()
        {
            List<RelatorioMovimentacaoVO> RelatorioMovimentacaoVOs = new List<RelatorioMovimentacaoVO>();

            // get Start (paging start index) and length (page size for paging)
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            string sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault(); ;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            string idAnalise = Request.Form.Get("idAnalise");

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int TotalRows = 0;

            if (!String.IsNullOrEmpty(idAnalise))
            {
                RelatorioMovimentacaoVOs = RecuperaDadosMovimentacao(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (RelatorioMovimentacaoVOs.Any())
                    TotalRows = RelatorioMovimentacaoVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = RelatorioMovimentacaoVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarRelatorioMovimentacao(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FileMovimetacao"] = SalvarExcelParticionado(NOME_ARQUIVO_MOVIMENTACAO, idAnalise, EnumTipoRelatorio.Movimentacao);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadRelatorioMovimentacao()
        {

            var arquivo = Session["FileMovimetacao"] as string;

            Session["FileMovimetacao"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_MOVIMENTACAO); 
        }

        private List<RelatorioMovimentacaoVO> RecuperaDadosMovimentacao(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@DisplayLength", length),
                    new SqlParameter("@DisplayStart", start),
                    new SqlParameter("@SortCol", sortColumn),
                    new SqlParameter("@SortDir", sortColumnDir),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos",listarTodos), 
                    new SqlParameter("@IdAnalise", idAnalise)
                };

            if (searchValue != "")
                param[4].Value = searchValue;

            List<RelatorioMovimentacaoVO> relatorioMovimentacaoVOs = db.Database.SqlQuery<RelatorioMovimentacaoVO>(
                    "EXEC PR_RELATORIO_MOVIMENTACAO @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return relatorioMovimentacaoVOs;
        }

        #endregion

        #region Relatório Levantamento

        public ActionResult Levantamento()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarRelatorioLevantamento()
        {
            List<RelatorioLevantamentoVO> RelatorioLevantamentoVOs = new List<RelatorioLevantamentoVO>();

            // get Start (paging start index) and length (page size for paging)
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            string sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault(); ;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            string idAnalise = Request.Form.Get("idAnalise");
            string CorLinha = Request.Form.Get("chkSelecionado");

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int TotalRows = 0;

            if (!String.IsNullOrEmpty(idAnalise))
            {
                RelatorioLevantamentoVOs = RecuperaDadosLevantamento(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise, CorLinha);

                if (RelatorioLevantamentoVOs.Any())
                    TotalRows = RelatorioLevantamentoVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = RelatorioLevantamentoVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarRelatorioLevantamento(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FileLevantamento"] = SalvarExcelParticionado(NOME_ARQUIVO_LEVANTAMENTO, idAnalise, EnumTipoRelatorio.Levantamento);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public JsonResult CalcularOmissaoEntradaSaida(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var dadosArquivo = RecuperaDadosLevantamento("0", "0", "0", "asc", "", "1", idAnalise.ToString(), null);

                var sumOES =
                    dadosArquivo.GroupBy(g => g.OMISSAO)
                        .Select(s => new { Omissao = s.Key, sum = s.Sum(x => x.BaseCalculo) });

                decimal sumOEntrada = 0;
                var omissaoEntrada = sumOES.FirstOrDefault(oe => oe.Omissao.ToUpper() == "ENTRADA");
                if (omissaoEntrada != null)
                {
                    sumOEntrada = omissaoEntrada.sum;
                }

                decimal sumOSaida = 0;
                var omissaoSaida = sumOES.FirstOrDefault(oe => oe.Omissao.ToUpper() == "SAÍDA");
                if (omissaoSaida != null)
                {
                    sumOSaida = omissaoSaida.sum;
                }

                result.Data = new { OEntrada = string.Format("{0:N}", sumOEntrada), OSaida = string.Format("{0:N}", sumOSaida), Status = (int)WebExceptionStatus.Success };
                //result.Data = new { OEntrada = sumOEntrada, OSaida = sumOSaida, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadRelatorioLevantamento()
        {

            var arquivo = Session["FileLevantamento"] as string;

            Session["FileLevantamento"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_LEVANTAMENTO); ;
        }

        public JsonResult ConstestarValor(string idsProdutoPai)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@IdProdutoPai", idsProdutoPai)
                };

                List<string> listPrdNaoContestados = db.Database.SqlQuery<string>("EXEC PR_ATUALIZAR_VALOR_PRODUTO_CONTESTADO @IdProdutoPai", param).ToList();

                result.Data = listPrdNaoContestados.Count > 0 ? new { Result = "O(s) produto(s) abaixo não foi contestado(s) por não possuírem - Entradas, Estoque Inicial e Final. </br> " + string.Join("</br>", listPrdNaoContestados), Status = (int)WebExceptionStatus.Success } :
                                                                new { Result = "Valor contestado com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public ActionResult ListarValoresContestar(string idAnalise, string idsProdutoPai)
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@IdAnalise", idAnalise),
                    new SqlParameter("@IdProdutoPai", idsProdutoPai)
                };

            List<ContestarValorManualVO> listPrdXPreco = db.Database.SqlQuery<ContestarValorManualVO>("EXEC PR_CONTESTAR_MANUALMENTE @IdAnalise, @IdProdutoPai", param).ToList();

            var result = from prd in listPrdXPreco
                         group prd by new { prd.Descricao, prd.IdProdutoPai } into Group
                         select new
                         {
                             Descricao = Group.Key.Descricao,
                             IdProdutoPai = Group.Key.IdProdutoPai,
                             Valores = Group.ToList()
                         };

            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SalvarContestacaoManual(IEnumerable<AllValores> valores)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                foreach (var item in valores)
                {
                    db.Database.ExecuteSqlCommand(string.Format(" UPDATE PRODUTO SET " +
                                                                "    Contestado = 1, " +
                                                                "    IdProdContesPlaMovSai = {0}, " +
                                                                "    IdProdContesPlaMovEnt = NULL, " +
                                                                "    IdProdContesPlaFinal = NULL, " +
                                                                "    IdProdContesPlaInicial = NULL " +
                                                                " WHERE Id = {1}", item.idValor, item.IdProdutoPai));
                }

                result.Data = new { Result = "Valores contestados com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        private List<RelatorioLevantamentoVO> RecuperaDadosLevantamento(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise, string CorLinha = "")
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@DisplayLength", length),
                    new SqlParameter("@DisplayStart", start),
                    new SqlParameter("@SortCol", sortColumn),
                    new SqlParameter("@SortDir", sortColumnDir),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos",listarTodos), 
                    new SqlParameter("@CorLinha",DBNull.Value),
                    new SqlParameter("@IdAnalise", idAnalise)
                };

            if (CorLinha == "1")
            {
                param[6].Value = 1;
            }
            else if (CorLinha == "2")
            {
                param[6].Value = 2;
            }

            if (searchValue != "")
                param[4].Value = searchValue;

            db.Database.CommandTimeout = 180;
            List<RelatorioLevantamentoVO> relatorioLevantamentoVOs = db.Database.SqlQuery<RelatorioLevantamentoVO>(
                    "EXEC PR_RELATORIO_LEVANTAMENTO @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @CorLinha, @IdAnalise", param).ToList();

            return relatorioLevantamentoVOs;
        }

        #endregion

        #region Relatório Venda abaixo do custo

        public ActionResult Venda()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarRelatorioVenda()
        {
            List<RelatorioVendaVO> RelatorioVendaVOs = new List<RelatorioVendaVO>();

            // get Start (paging start index) and length (page size for paging)
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            string sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault(); ;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            string idAnalise = Request.Form.Get("idAnalise");

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int TotalRows = 0;

            if (!String.IsNullOrEmpty(idAnalise))
            {
                RelatorioVendaVOs = RecuperaDadosVenda(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (RelatorioVendaVOs.Any())
                    TotalRows = RelatorioVendaVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = RelatorioVendaVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarRelatorioVenda(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FileVenda"] = SalvarExcelParticionado(NOME_ARQUIVO_VENDA, idAnalise, EnumTipoRelatorio.Venda);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public JsonResult ValorTotalICMS(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var dadosArquivo = RecuperaDadosVenda("0", "0", "0", "asc", "", "1", idAnalise.ToString());

                var sumTotalICMS = dadosArquivo.Sum(x => x.ICMSEstornar);

                result.Data = new { TotalICMS = string.Format("{0:N}", sumTotalICMS), Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        private List<RelatorioVendaVO> RecuperaDadosVenda(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@DisplayLength", length),
                    new SqlParameter("@DisplayStart", start),
                    new SqlParameter("@SortCol", sortColumn),
                    new SqlParameter("@SortDir", sortColumnDir),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos",listarTodos), 
                    new SqlParameter("@IdAnalise", idAnalise)
                };

            if (searchValue != "")
                param[4].Value = searchValue;

            db.Database.CommandTimeout = 180;
            List<RelatorioVendaVO> relatorioVendaVOs = db.Database.SqlQuery<RelatorioVendaVO>(
                    "EXEC PR_RELATORIO_VENDAS_ABAIXO_DO_CUSTO @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return relatorioVendaVOs;
        }

        [HttpGet]
        public FileResult DownloadRelatorioVenda()
        {

            var arquivo = Session["FileVenda"] as string;

            Session["FileVenda"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_VENDA); ;
        }

        #endregion

        #region Relatório Preço Unitário

        public ActionResult PrecoUnitario()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarRelatorioPrecoUnitario()
        {
            List<RelatorioPrecoUnitarioVO> relatorioPrecoUnitarioVos = new List<RelatorioPrecoUnitarioVO>();

            // get Start (paging start index) and length (page size for paging)
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            string sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault(); ;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            string idAnalise = Request.Form.Get("idAnalise");
            int TotalRows = 0;

            if (!String.IsNullOrEmpty(idAnalise))
            {
                relatorioPrecoUnitarioVos = RecuperaDadosRelatorioPrecoUnitario(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (relatorioPrecoUnitarioVos.Any())
                    TotalRows = relatorioPrecoUnitarioVos.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = relatorioPrecoUnitarioVos }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ExportarRelatorioPrecoUnitario(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FilePreco"] = SalvarExcelParticionado(NOME_ARQUIVO_PRECOUNITARIO, idAnalise, EnumTipoRelatorio.PrecoUnitario);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        private List<RelatorioPrecoUnitarioVO> RecuperaDadosRelatorioPrecoUnitario(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@DisplayLength", length),
                    new SqlParameter("@DisplayStart", start),
                    new SqlParameter("@SortCol", sortColumn),
                    new SqlParameter("@SortDir", sortColumnDir),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos",listarTodos), 
                    new SqlParameter("@IdAnalise", idAnalise)
                };

            if (searchValue != "")
                param[4].Value = searchValue;

            db.Database.CommandTimeout = 180;
            List<RelatorioPrecoUnitarioVO> relatorioPrecoUnitarioVOs = db.Database.SqlQuery<RelatorioPrecoUnitarioVO>(
                    "EXEC PR_RELATORIO_PRECO_UNITARIO @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return relatorioPrecoUnitarioVOs;
        }

        [HttpGet]
        public FileResult DownloadRelatorioPrecoUnitario()
        {

            var arquivo = Session["FilePreco"] as string;

            Session["FilePreco"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_PRECOUNITARIO); ;
        }

        #endregion

        private DataTable FormatarDataTable(DataTable dt, EnumTipoRelatorio tipoRelatorio)
        {

            if (dt.Columns.Contains("TotalCount"))
                dt.Columns.Remove("TotalCount");

            switch (tipoRelatorio)
            {
                case EnumTipoRelatorio.Movimentacao:

                    dt.Columns["ProdutoDescricao"].ColumnName = "Descrição Original";
                    dt.Columns["ProdutoUnificado"].ColumnName = "Descrição Unificada";
                    dt.Columns["UnidadeDescricao"].ColumnName = "Unidade";
                    dt.Columns["QtdInicial"].ColumnName = "Est. Inicial";
                    dt.Columns["QtdEntrada"].ColumnName = "Entradas";
                    dt.Columns["QtdSaida"].ColumnName = "Saídas";
                    dt.Columns["QtdFinal"].ColumnName = "Est. Final";
                    dt.Columns["ValorInicial"].ColumnName = "Vlr Unit Inicial";
                    dt.Columns["ValorEntrada"].ColumnName = "Vlr Unit Entrada";
                    dt.Columns["ValorSaida"].ColumnName = "Vlr Unit Saída";
                    dt.Columns["ValorFinal"].ColumnName = "Vlr Unit Final";

                    break;
                case EnumTipoRelatorio.Venda:

                    dt.Columns["SEF_NFE_ECF"].ColumnName = "SEF/NFE/ECF";
                    dt.Columns["NUM_NF"].ColumnName = "Número NF/COO";
                    dt.Columns["DataEmissao"].ColumnName = "Data de Emissão";
                    dt.Columns["Descricao"].ColumnName = "Descrição Original";
                    dt.Columns["DescricaoUnificada"].ColumnName = "Descrição Unificada";
                    dt.Columns["Quantidade"].ColumnName = "Quantidade";
                    dt.Columns["DescricaoUnidade"].ColumnName = "Unidade";
                    dt.Columns["ValorUnitario"].ColumnName = "Valor Unitário";
                    dt.Columns["ValorTotal"].ColumnName = "Valor Total";
                    dt.Columns["CFOP"].ColumnName = "CFOP Saída";
                    dt.Columns["NFEntrada"].ColumnName = "NF Entrada";
                    dt.Columns["DataEntradaMenor"].ColumnName = "Data de Entrada";
                    dt.Columns["CFOPEntrada"].ColumnName = "CFOP Entrada";
                    dt.Columns["ValorUnitarioMenor"].ColumnName = "Vlr Unit Mínimo";
                    dt.Columns["ValorUnitarioDiferenca"].ColumnName = "Diferença Valor Unit";
                    dt.Columns["BCEstorno"].ColumnName = "BC a Estornar";
                    dt.Columns["ICMSEstornar"].ColumnName = "ICMS a Estornar";

                    break;
                case EnumTipoRelatorio.PrecoUnitario:
                    dt.Columns["Descricao"].ColumnName = "Descrição Unificada";
                    dt.Columns["ValorUnitario"].ColumnName = "Valor Unitário";
                    dt.Columns["PlanilhaOrigem"].ColumnName = "Origem";
                    dt.Columns["NUM_NF"].ColumnName = "Num. NF";
                    dt.Columns["DataEntrada"].ColumnName = "Data de Emissão";

                    break;

                case EnumTipoRelatorio.Levantamento:

                    if (dt.Columns.Contains("IdProdutoPai"))
                        dt.Columns.Remove("IdProdutoPai");

                    if (dt.Columns.Contains("CorLinha"))
                        dt.Columns.Remove("CorLinha");

                    dt.Columns["ProdutoDescricao"].ColumnName = "Descrição Unificada";
                    dt.Columns["UnidadeDescricao"].ColumnName = "Unid";
                    dt.Columns["QtdInicial"].ColumnName = "Estoque Inicial SEF";
                    dt.Columns["QtdEntrada"].ColumnName = "Entradas";
                    dt.Columns["QtdSaida"].ColumnName = "Saídas";
                    dt.Columns["QtdFinalCalculado"].ColumnName = "Estoque Final Calculado";
                    dt.Columns["QtdFinal"].ColumnName = "Estoque Final SEF";
                    dt.Columns["QtdDiferenca"].ColumnName = "Diferença";
                    dt.Columns["OMISSAO"].ColumnName = "Omissão";
                    dt.Columns["ValorUnitario"].ColumnName = "Valor Unitário (R$)";
                    dt.Columns["BaseCalculo"].ColumnName = "Base Cálculo (R$)";

                    break;

            }

            return dt;
        }

        /// <summary>
        /// Gerar um arquivo a partir de um datatable
        /// </summary>
        /// <param name="data">Dados</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>Caminho do arquivo</returns>
        private string SalvarExcel(DataTable data, string nomeArquivo)
        {
            DataGrid grid = new DataGrid();

            grid.HeaderStyle.Font.Bold = true;
            grid.DataSource = data;
            grid.DataMember = data.TableName;
            grid.DataBind();

            var extencaoArquivo = Path.GetExtension(nomeArquivo);

            var novoNome = Path.Combine(Server.MapPath("~/Download/")) + nomeArquivo.Remove((nomeArquivo.Length - extencaoArquivo.Length) - 1, extencaoArquivo.Length + 1);
            novoNome += HttpContext.Session.SessionID + ".xls";

            // render the DataGrid control to a file
            var path = Path.Combine(Server.MapPath("~/Download/"), novoNome);
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    grid.RenderControl(hw);
                }
            }

            return novoNome;
        }

        /// <summary>
        /// Gerar um arquivo zip a partir de um datatable
        /// </summary>
        /// <param name="data">Dados</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>Caminho do arquivo</returns>
        private string SalvarExcelParticionado(string nomeArquivo, int idAnalise, EnumTipoRelatorio tipoRelatorio)
        {
            var novoNome = string.Empty;

            Guid guid = Guid.NewGuid();

            string diretorio = Path.Combine(Server.MapPath("~/Download/"), HttpContext.Session.SessionID);
            string arquivoZip = Path.Combine(Server.MapPath("~/Download/"), guid.ToString() + ".zip");

            DataTable dt = new DataTable();

            if (Directory.Exists(diretorio))
                Directory.Delete(diretorio, true);

            Directory.CreateDirectory(diretorio);

            bool continuarImportacao = true;

            for (int i = 0; continuarImportacao; i++)
            {
                novoNome = Path.Combine(diretorio,
                    Path.GetFileNameWithoutExtension(nomeArquivo) + "_Parte_" + (i + 1).ToString() + ".xls");

                using (StreamWriter sw = new StreamWriter(novoNome, false, Encoding.UTF8))
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        switch (tipoRelatorio)
                        {
                            case EnumTipoRelatorio.Desconhecido:
                                throw new Exception("Tipo de relatório inválido.");

                            case EnumTipoRelatorio.Levantamento:
                                List<RelatorioLevantamentoVO> relatorioLevantamentoVOs =
                                    RecuperaDadosLevantamento(Util.quantidadeQuebraExcel.ToString(),
                                        (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = relatorioLevantamentoVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(relatorioLevantamentoVOs.AsDataTable(), tipoRelatorio);
                                break;

                            case EnumTipoRelatorio.Movimentacao:
                                List<RelatorioMovimentacaoVO> relatorioMovimentacaoVOs =
                                    RecuperaDadosMovimentacao(Util.quantidadeQuebraExcel.ToString(), (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = relatorioMovimentacaoVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(relatorioMovimentacaoVOs.AsDataTable(), tipoRelatorio);
                                break;

                            case EnumTipoRelatorio.PrecoUnitario:
                                List<RelatorioPrecoUnitarioVO> relatorioPrecoUnitarioVOs =
                                    RecuperaDadosRelatorioPrecoUnitario(Util.quantidadeQuebraExcel.ToString(), (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0",
                                        idAnalise.ToString());
                                continuarImportacao = relatorioPrecoUnitarioVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(relatorioPrecoUnitarioVOs.AsDataTable(), tipoRelatorio);
                                break;

                            case EnumTipoRelatorio.Venda:
                                List<RelatorioVendaVO> relatorioVendaVOs =
                                    RecuperaDadosVenda(Util.quantidadeQuebraExcel.ToString(),
                                        (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = relatorioVendaVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(relatorioVendaVOs.AsDataTable(), tipoRelatorio);
                                break;
                        }

                        if (continuarImportacao)
                        {
                            GridView gridView = new GridView();
                            gridView.DataSource = dt;
                            gridView.HeaderStyle.Font.Bold = true;
                            gridView.DataBind();
                            gridView.RenderControl(hw);
                        }
                    }
                }

                if (!continuarImportacao)
                {
                    if (System.IO.File.Exists(novoNome))
                        System.IO.File.Delete(novoNome);
                }

            }

            if (System.IO.File.Exists(arquivoZip))
                System.IO.File.Delete(arquivoZip);

            ZipFile.CreateFromDirectory(diretorio, arquivoZip);

            return arquivoZip;
        }
    }
}
