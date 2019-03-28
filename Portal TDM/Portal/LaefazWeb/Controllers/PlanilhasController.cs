using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
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
    public class PlanilhasController : Controller
    {
        private DbEntities db = new DbEntities();

        private const string NOME_ARQUIVO_MOVIMENTACAO_SAIDA = "LAEFAZ_Saidas.zip";
        private const string NOME_ARQUIVO_MOVIMENTACAO_ENTRADA = "LAEFAZ_Entradas.zip";
        private const string NOME_ARQUIVO_ESTOQUE_INICIAL = "LAEFAZ_EstoqueInicial.xls";
        private const string NOME_ARQUIVO_ESTOQUE_FINAL = "LAEFAZ_EstoqueFinal.xls";
        private const string NOME_ARQUIVO_PRODUTO = "LAEFAZ_Produtos.xls";

        #region Relatório Estoque Inicial

        public ActionResult EstoqueInicial()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarPlanilhaEstoqueInicial()
        {
            List<PlanilhaEstoqueInicialVO> PlanilhaEstoqueInicialVOs = new List<PlanilhaEstoqueInicialVO>();

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
                PlanilhaEstoqueInicialVOs = RecuperaDadosEstoqueInicial(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (PlanilhaEstoqueInicialVOs.Any())
                    TotalRows = PlanilhaEstoqueInicialVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = PlanilhaEstoqueInicialVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarPlanilhaEstoqueInicial(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var dadosArquivo = RecuperaDadosEstoqueInicial("0", "0", "0", "asc", "", "1", idAnalise.ToString());
                var dt = FormatarDataTable(dadosArquivo.AsDataTable(), EnumTipoRelatorio.EstoqueInicial);
                Session["FileEstoqueInicial"] = SalvarExcel(dt, NOME_ARQUIVO_ESTOQUE_INICIAL);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadPlanilhaEstoqueInicial()
        {

            var arquivo = Session["FileEstoqueInicial"] as string;

            Session["FileEstoqueInicial"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_ESTOQUE_INICIAL); ;
        }

        private List<PlanilhaEstoqueInicialVO> RecuperaDadosEstoqueInicial(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
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

            List<PlanilhaEstoqueInicialVO> PlanilhaEstoqueInicialVOs = db.Database.SqlQuery<PlanilhaEstoqueInicialVO>(
                    "EXEC PR_PLANILHA_ESTOQUE_INICIAL @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return PlanilhaEstoqueInicialVOs;
        }

        #endregion

        #region Relatório Estoque Final

        public ActionResult EstoqueFinal()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarPlanilhaEstoqueFinal()
        {
            List<PlanilhaEstoqueFinalVO> PlanilhaEstoqueFinalVOs = new List<PlanilhaEstoqueFinalVO>();

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
                PlanilhaEstoqueFinalVOs = RecuperaDadosEstoqueFinal(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (PlanilhaEstoqueFinalVOs.Any())
                    TotalRows = PlanilhaEstoqueFinalVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = PlanilhaEstoqueFinalVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarPlanilhaEstoqueFinal(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var dadosArquivo = RecuperaDadosEstoqueFinal("0", "0", "0", "asc", "", "1", idAnalise.ToString());
                var dt = FormatarDataTable(dadosArquivo.AsDataTable(), EnumTipoRelatorio.EstoqueFinal);
                Session["FileEstoqueFinal"] = SalvarExcel(dt, NOME_ARQUIVO_ESTOQUE_FINAL);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadPlanilhaEstoqueFinal()
        {

            var arquivo = Session["FileEstoqueFinal"] as string;

            Session["FileEstoqueFinal"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_ESTOQUE_FINAL); ;
        }

        private List<PlanilhaEstoqueFinalVO> RecuperaDadosEstoqueFinal(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
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

            List<PlanilhaEstoqueFinalVO> PlanilhaEstoqueFinalVOs = db.Database.SqlQuery<PlanilhaEstoqueFinalVO>(
                    "EXEC PR_PLANILHA_ESTOQUE_Final @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return PlanilhaEstoqueFinalVOs;
        }

        #endregion

        #region Relatório Movimentação Entrada

        public ActionResult MovimentacaoEntrada()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarPlanilhaMovimentacaoEntrada()
        {
            List<PlanilhaMovimentacaoEntradaVO> PlanilhaMovimentacaoEntradaVOs = new List<PlanilhaMovimentacaoEntradaVO>();

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
                PlanilhaMovimentacaoEntradaVOs = RecuperaDadosMovimentacaoEntrada(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (PlanilhaMovimentacaoEntradaVOs.Any())
                    TotalRows = PlanilhaMovimentacaoEntradaVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = PlanilhaMovimentacaoEntradaVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarPlanilhaMovimentacaoEntrada(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FileMovimentacaoEntrada"] = SalvarExcelParticionado(NOME_ARQUIVO_MOVIMENTACAO_ENTRADA, idAnalise, EnumTipoArquivo.MovimentacaoEntrada);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadPlanilhaMovimentacaoEntrada()
        {

            var arquivo = Session["FileMovimentacaoEntrada"] as string;

            Session["FileMovimentacaoEntrada"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_MOVIMENTACAO_ENTRADA); ;
        }

        private List<PlanilhaMovimentacaoEntradaVO> RecuperaDadosMovimentacaoEntrada(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
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

            List<PlanilhaMovimentacaoEntradaVO> PlanilhaMovimentacaoEntradaVOs = db.Database.SqlQuery<PlanilhaMovimentacaoEntradaVO>(
                    "EXEC PR_PLANILHA_MOVIMENTACAO_ENTRADA @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return PlanilhaMovimentacaoEntradaVOs;
        }

        #endregion

        #region Relatório Movimentação Saída

        public ActionResult MovimentacaoSaida()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarPlanilhaMovimentacaoSaida()
        {
            List<PlanilhaMovimentacaoSaidaVO> PlanilhaMovimentacaoSaidaVOs = new List<PlanilhaMovimentacaoSaidaVO>();

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
                PlanilhaMovimentacaoSaidaVOs = RecuperaDadosMovimentacaoSaida(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (PlanilhaMovimentacaoSaidaVOs.Any())
                    TotalRows = PlanilhaMovimentacaoSaidaVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = PlanilhaMovimentacaoSaidaVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarPlanilhaMovimentacaoSaida(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FileMovimentacaoSaida"] = SalvarExcelParticionado(NOME_ARQUIVO_MOVIMENTACAO_SAIDA, idAnalise, EnumTipoArquivo.MovimentacaoSaida);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadPlanilhaMovimentacaoSaida()
        {
            var arquivo = Session["FileMovimentacaoSaida"] as string;

            Session["FileMovimentacaoSaida"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_MOVIMENTACAO_SAIDA); ;
        }

        private List<PlanilhaMovimentacaoSaidaVO> RecuperaDadosMovimentacaoSaida(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
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

            List<PlanilhaMovimentacaoSaidaVO> PlanilhaMovimentacaoSaidaVOs = db.Database.SqlQuery<PlanilhaMovimentacaoSaidaVO>(
                    "EXEC PR_PLANILHA_MOVIMENTACAO_SAIDA @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return PlanilhaMovimentacaoSaidaVOs;
        }

        #endregion

        #region Relatório Produtos

        public ActionResult Produto()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View();
        }

        public JsonResult CarregarPlanilhaProduto()
        {
            List<PlanilhaProdutoVO> PlanilhaProdutoVOs = new List<PlanilhaProdutoVO>();

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
                PlanilhaProdutoVOs = RecuperaDadosProduto(length, start, sortColumn, sortColumnDir, searchValue, "0", idAnalise);

                if (PlanilhaProdutoVOs.Any())
                    TotalRows = PlanilhaProdutoVOs.FirstOrDefault().TotalCount;
            }

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = PlanilhaProdutoVOs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportarPlanilhaProduto(int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var dadosArquivo = RecuperaDadosProduto("0", "0", "0", "asc", "", "1", idAnalise.ToString());
                var dt = FormatarDataTable(dadosArquivo.AsDataTable(), EnumTipoRelatorio.Produto);
                Session["FileProduto"] = SalvarExcel(dt, NOME_ARQUIVO_PRODUTO);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadPlanilhaProduto()
        {
            var arquivo = Session["FileProduto"] as string;

            Session["FileProduto"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO_PRODUTO); ;
        }

        private List<PlanilhaProdutoVO> RecuperaDadosProduto(string length, string start, string sortColumn, string sortColumnDir, string searchValue, string listarTodos, string idAnalise)
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

            List<PlanilhaProdutoVO> PlanilhaProdutoVOs = db.Database.SqlQuery<PlanilhaProdutoVO>(
                    "EXEC PR_PLANILHA_PRODUTO @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @LISTARTODOS, @IdAnalise", param).ToList();

            return PlanilhaProdutoVOs;
        }

        #endregion

        private DataTable FormatarDataTable(DataTable dt, EnumTipoRelatorio tipoRelatorio)
        {

            if (dt.Columns.Contains("TotalCount"))
                dt.Columns.Remove("TotalCount");

            switch (tipoRelatorio)
            {
                case EnumTipoRelatorio.EstoqueInicial:
                case EnumTipoRelatorio.EstoqueFinal:
                    dt.Columns["ProdutoDescricao"].ColumnName = "Descrição Original";
                    dt.Columns["ProdutoUnificado"].ColumnName = "Descrição Unificada";
                    dt.Columns["Quantidade"].ColumnName = "Quantidade";
                    dt.Columns["UnidadeDescricao"].ColumnName = "Unidade";
                    dt.Columns["ValorUnitario"].ColumnName = "Valor Unitário";
                    dt.Columns["ValorTotal"].ColumnName = "Valor Total";

                    break;
                case EnumTipoRelatorio.MovimentacaoEntrada:
                    dt.Columns["SEF_NFE_ECF"].ColumnName = "SEF/NFE";
                    dt.Columns["NUM_NF"].ColumnName = "Número NF";
                    dt.Columns["DataEntrada"].ColumnName = "Data de Entrada";
                    dt.Columns["ProdutoDescricao"].ColumnName = "Descrição Original";
                    dt.Columns["ProdutoUnificado"].ColumnName = "Descrição Unificada";
                    dt.Columns["Quantidade"].ColumnName = "Quantidade";
                    dt.Columns["UnidadeDescricao"].ColumnName = "Unidade";
                    dt.Columns["ValorUnitario"].ColumnName = "Valor Unitário";
                    dt.Columns["ValorTotal"].ColumnName = "Valor Total";
                    dt.Columns["CFOP"].ColumnName = "CFOP";

                    break;
                case EnumTipoRelatorio.MovimentacaoSaida:
                    dt.Columns["SEF_NFE_ECF"].ColumnName = "SEF/NFE/ECF";
                    dt.Columns["NUM_NF"].ColumnName = "Número NF/COO";
                    dt.Columns["DataEntrada"].ColumnName = "Data de Emissão";
                    dt.Columns["ProdutoDescricao"].ColumnName = "Descrição Original";
                    dt.Columns["ProdutoUnificado"].ColumnName = "Descrição Unificada";
                    dt.Columns["Quantidade"].ColumnName = "Quantidade";
                    dt.Columns["UnidadeDescricao"].ColumnName = "Unidade";
                    dt.Columns["ValorUnitario"].ColumnName = "Valor Unitário";
                    dt.Columns["ValorTotal"].ColumnName = "Valor Total";
                    dt.Columns["CFOP"].ColumnName = "CFOP";

                    break;
                case EnumTipoRelatorio.Produto:
                    dt.Columns["DescricaoOriginal"].ColumnName = "Descrição Original";
                    dt.Columns["DescricaoUnificada"].ColumnName = "Descrição Unificada";
                    dt.Columns["DescricaoUnidade"].ColumnName = "Unidade";

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
        /// Gerar um arquivo a partir de um datatable
        /// </summary>
        /// <param name="data">Dados</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>Caminho do arquivo</returns>
        private string SalvarExcelParticionado(string nomeArquivo, int idAnalise, EnumTipoArquivo tipoArquivo)
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
                novoNome = Path.Combine(diretorio, Path.GetFileNameWithoutExtension(nomeArquivo) + "_Parte_" + (i + 1).ToString() + ".xls");

                using (StreamWriter sw = new StreamWriter(novoNome, false, Encoding.UTF8))
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        switch (tipoArquivo)
                        {
                            case EnumTipoArquivo.Desconhecido:
                                throw new Exception("Tipo de arquivo inválido.");

                            case EnumTipoArquivo.MovimentacaoEntrada:
                                List<PlanilhaMovimentacaoEntradaVO> planilhaMovimentacaoEntradaVOs = RecuperaDadosMovimentacaoEntrada(Util.quantidadeQuebraExcel.ToString(), (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = planilhaMovimentacaoEntradaVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(planilhaMovimentacaoEntradaVOs.AsDataTable(), EnumTipoRelatorio.MovimentacaoEntrada);
                                break;

                            case EnumTipoArquivo.MovimentacaoSaida:
                                List<PlanilhaMovimentacaoSaidaVO> planilhaMovimentacaoSaidaVOs = RecuperaDadosMovimentacaoSaida(Util.quantidadeQuebraExcel.ToString(), (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = planilhaMovimentacaoSaidaVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(planilhaMovimentacaoSaidaVOs.AsDataTable(), EnumTipoRelatorio.MovimentacaoSaida);
                                break;

                            case EnumTipoArquivo.EstoqueFinal:
                                List<PlanilhaEstoqueFinalVO> planilhaEstoqueFinalVOs = RecuperaDadosEstoqueFinal(Util.quantidadeQuebraExcel.ToString(), (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = planilhaEstoqueFinalVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(planilhaEstoqueFinalVOs.AsDataTable(), EnumTipoRelatorio.EstoqueFinal);
                                break;

                            case EnumTipoArquivo.EstoqueInicial:
                                List<PlanilhaEstoqueInicialVO> planilhaEstoqueInicialVOs = RecuperaDadosEstoqueInicial(Util.quantidadeQuebraExcel.ToString(), (i * Util.quantidadeQuebraExcel).ToString(), "0", "asc", "", "0", idAnalise.ToString());
                                continuarImportacao = planilhaEstoqueInicialVOs.Any();
                                if (continuarImportacao)
                                    dt = FormatarDataTable(planilhaEstoqueInicialVOs.AsDataTable(), EnumTipoRelatorio.EstoqueInicial);
                                break;
                        }

                        if (continuarImportacao)
                        {
                            GridView gridView = new GridView();
                            DataTable dtn = new DataTable();
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