using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using TDMWeb.Exceptions;
using System.Linq;
using System.Web.Mvc;
using TDMWeb.Models;
using System.Net;
using System.Text;
using System.Web.UI;
using TDMWeb.Enumerators;
using TDMWeb.Extensions;
using TDMWeb.ViewModel;
using TDMWeb.Models.VOs;
using TDMWeb.Lib;
using System.Web.UI.WebControls;

namespace TDMWeb.Controllers
{
    [UsuarioLogado]
    public class ProdutosController : Controller
    {
        private DbEntities db = new DbEntities();

        private const string NOME_ARQUIVO = "LAEFAZ.xls";
        //
        // GET: /Produtos/


        public ActionResult Index()
        {
            ViewBag.ListaAnalises = db.Analise.ToList();
            return View(new List<Produto>()); // db.Produtos.Where(p => p.Unificar == true && p.Ativo == true).ToList()
        }

        public JsonResult CarregarProdutos()
        {
            List<ProdutoVO> produtosVOs = new List<ProdutoVO>();

            // get Start (paging start index) and length (page size for paging)
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            string sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault(); ;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            string idAnalise = Request.Form.Get("idAnalise");
            string Ativo = Request.Form.Get("StatusAtivo");
            string tipoUnificacao = Request.Form.Get("chkSelecionado");

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int TotalRows = 0;

            if (!String.IsNullOrEmpty(idAnalise))
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@DisplayLength", length),
                    new SqlParameter("@DisplayStart", start),
                    new SqlParameter("@SortCol", sortColumn),
                    new SqlParameter("@SortDir", sortColumnDir),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@Ativo", Ativo),
                    new SqlParameter("@ListarTodos","0"),
                    new SqlParameter("@TipoUnificacao",DBNull.Value),
                    new SqlParameter("@IdAnalise", idAnalise)
                };

                if (tipoUnificacao == "1")
                {
                    param[7].Value = 0;
                }
                else if (tipoUnificacao == "2")
                {
                    param[7].Value = 1;
                }

                if (searchValue != "")
                    param[4].Value = searchValue;

                produtosVOs = db.Database.SqlQuery<ProdutoVO>(
                        "EXEC PR_LISTAR_PRODUTOS @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @Ativo, @ListarTodos, @TipoUnificacao, @IdAnalise ", param).ToList();

                if (produtosVOs.Any())
                    TotalRows = produtosVOs.FirstOrDefault().TotalCount;

            }
            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = produtosVOs }, JsonRequestBehavior.AllowGet);
        }

        private int GetAnaliseTotalCount()
        {
            int totalProdutos = 0;

            using (SqlConnection con = new SqlConnection(Util.SqlConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Select count(*) from Produto", con);
                con.Open();
                totalProdutos = (int)cmd.ExecuteScalar();
            }

            return totalProdutos;
        }

        public JsonResult AtivarDesativarProduto(int idAnalise, string idsProdutos, bool status)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                DesativaAtivarProduto(idAnalise, idsProdutos, status);
                result.Data = new { Result = string.Format("Produto {0} com sucesso.", status == true ? "ativado" : "desativado"), Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }

            return result;
        }

        [HttpPost]
        public ActionResult Unificar(string idsProdutos, int idUnidade, string descUnificada, int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {

                UnificarProdutos(idsProdutos, descUnificada, idAnalise, idUnidade);

                result.Data = new { Result = "Unificação Realizada com Sucesso.", Status = (int)WebExceptionStatus.Success };

            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }

            return result;
        }

        /// <summary>
        /// Exportar Produto para Excel
        /// </summary>
        /// <param name="ativo">Status do produto </param>
        /// <param name="idAnalise">Id da analise selecionada</param>
        public ActionResult ExportarProdutos(bool ativo, int idAnalise)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Session["FileProduto"] = GerarAquivo(ativo, idAnalise);
                result.Data = new { Result = (int)WebExceptionStatus.Success, Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        [HttpGet]
        public FileResult DownloadProdutos()
        {

            var arquivo = Session["FileProduto"] as string;

            Session["FileProduto"] = null;

            FileStream stream = new FileStream(arquivo, FileMode.Open);

            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, NOME_ARQUIVO);
        }

        [HttpPost]
        public JsonResult ImportarProdutos()
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var idAnalise = Request.Form["idAnalise"];
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        file.SaveAs(path);

                        SalvarImportacao(path, Convert.ToInt32(idAnalise));

                        //Remove o arquivo
                        System.IO.File.Delete(path);

                        result.Data = new { Result = "O arquivo importado com sucesso.", Status = (int)WebExceptionStatus.Success };
                    }
                    else
                    {
                        result.Data = new { Result = "O arquivo informado está vazio.", Status = (int)WebExceptionStatus.UnknownError };
                    }
                }
                else
                {
                    result.Data = new { Result = "Nenhum arquivo encontrado.", Status = (int)WebExceptionStatus.UnknownError };
                }

            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }

            return result;
        }

        private void SalvarImportacao(string path, int idAnalise)
        {
            DataTable dt;

            dt = Util.LerExcel(path, EnumTipoArquivo.ProdutoExcel);

            var produtos = db.Produto.Where(p => p.IdAnalise == idAnalise).ToList();

            long chave = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));

            //Recupera todas Unidades
            var unidades = db.Unidade.ToList();

            var distinctProdutos = (from p in dt.AsEnumerable()
                                    where p[(int)EnumImportacaoProduto.DescricaoUnificada].ToString().Trim() != ""
                                    select new
                                    {
                                        //Id = p[(int) EnumImportacaoProduto.Idproduto],
                                        Descricao = p[(int)EnumImportacaoProduto.DescricaoUnificada] == null ? "" : p[(int)EnumImportacaoProduto.DescricaoUnificada].ToString(),
                                        // ReSharper disable once PossibleNullReferenceException
                                        IdUnidade =
                                            unidades.FirstOrDefault(uni => uni.Descricao == p[(int)EnumImportacaoProduto.Unidade].ToString())
                                                .Id,
                                        Ativo = true,
                                        IdAnalise = idAnalise,
                                        Unificar = 0
                                    }).Distinct().ToList();


            //Recupera os produtos que precisam ser inseridos.
            var prodNovos =
                distinctProdutos
                    .Where(p =>
                        !produtos.Select(prod => string.Concat(prod.Descricao, prod.IdUnidade))
                            .ToList()
                            .Contains(string.Concat(p.Descricao, p.IdUnidade))
                    ).ToList();

            if (prodNovos.Any())
            {
                Util.InserirProduto(prodNovos.AsDataTable());

                produtos = db.Produto.Where(p => p.IdAnalise == idAnalise).ToList();
            }

            var produtoImportacao = (from prd in dt.AsEnumerable()
                                     where prd[(int)EnumImportacaoProduto.DescricaoUnificada].ToString().Trim() != ""
                                     select new
                                     {
                                         IdProduto = prd[(int)EnumImportacaoProduto.Idproduto],
                                         IdProdutoPai =
                                             // ReSharper disable once PossibleNullReferenceException
                                             produtos.FirstOrDefault(
                                                 p => p.Descricao == prd[(int)EnumImportacaoProduto.DescricaoUnificada].ToString()).Id,
                                         Chave = chave
                                     }).ToList();

            if (produtoImportacao.Any())
            {
                using (SqlBulkCopy sqlBulk = new SqlBulkCopy(Util.SqlConnectionString))
                {
                    sqlBulk.ColumnMappings.Clear();
                    sqlBulk.ColumnMappings.Add("IdProduto", "IdProduto");
                    sqlBulk.ColumnMappings.Add("IdProdutoPai", "IdProdutoPai");
                    sqlBulk.ColumnMappings.Add("Chave", "Chave");
                    sqlBulk.DestinationTableName = "ImportacaoProdutos";
                    sqlBulk.WriteToServer(produtoImportacao.AsDataTable());
                    sqlBulk.Close();
                }

                //Chamar Procedure de unificar
                SqlParameter[] parametros =
                        {
                            new SqlParameter("@IdAnalise", idAnalise),
                            new SqlParameter("@Chave", chave)
                        };

                var retorno = db.Database.ExecuteSqlCommand("EXEC PR_ATUALIZAR_PRODUTOS_EM_LOTE @IdAnalise, @Chave", parametros);
            }

        }

        private string GerarAquivo(bool ativo, int idAnalise)
        {
            List<ProdutoExcel> listaProdutos = (from produto in db.Produto
                                                where produto.Ativo == ativo && produto.IdAnalise == idAnalise && produto.Unificar == true
                                                select (new ProdutoExcel()
                                                {
                                                    Id = produto.Id,
                                                    DescricaoOriginalProduto = produto.Descricao,
                                                    //DescricaoUnificada = produto.ProdutoPai != null ? produto.ProdutoPai.Descricao : "",
                                                    Unidade = produto.Unidade.Descricao,
                                                    JaUnificado = produto.Unificado
                                                })).ToList();


            return SalvarExcel(FormatarDataTable(listaProdutos.AsDataTable(),EnumTipoArquivo.ProdutoExcel), NOME_ARQUIVO);

        }

        private void UnificarProdutos(string idsProdutos, string descricaoUnificada, int idAnalise, int idUnidade)
        {
            //Se a descrição unificado for em branco é para desunificar os produtos.
            if (descricaoUnificada == null || descricaoUnificada.Trim().Equals(string.Empty))
            {
                db.Database.ExecuteSqlCommand(string.Format("UPDATE produto SET IdProdutoPai = NULL, Unificado = 0 WHERE IdAnalise = {0} AND Id in ({1})", idAnalise, idsProdutos));
                return;
            }
            var prdoutoUnificado =
                db.Produto.FirstOrDefault(p => p.Descricao == descricaoUnificada.ToUpper() && p.IdAnalise == idAnalise);

            //Se a descrição unificada não existir cria um novo produto.
            if (prdoutoUnificado == null)
            {
                prdoutoUnificado = new Produto()
                {
                    IdAnalise = idAnalise,
                    Descricao = descricaoUnificada.ToUpper(),
                    IdUnidade = idUnidade,
                    Ativo = true,
                    Unificar = false
                };

                db.Produto.Add(prdoutoUnificado);
                db.SaveChanges();
            }

            //Antes de fazer update validar se o produto pai já existe em algum outro produto com unidade diferente.
            var unidadeDiferente = db.Produto.Any(p => p.IdProdutoPai == prdoutoUnificado.Id && p.IdUnidade != idUnidade);

            if (unidadeDiferente)
                throw new UnificacaoInvalidaException(string.Format("Produto {0} já está unificado com produto(s) de unidade diferente", prdoutoUnificado.Descricao));

            var sql = string.Format("UPDATE produto SET IdProdutoPai = {0}, Unificado = 1 WHERE IdAnalise = {1} AND Id in ({2})", prdoutoUnificado.Id, idAnalise, idsProdutos);
            db.Database.ExecuteSqlCommand(sql);

        }

        private void DesativaAtivarProduto(int idAnalise, string idsProdutos, bool status)
        {

            var sql = string.Format("UPDATE produto SET Ativo = {0} WHERE IdAnalise = {1} AND Id in ({2})", status == true ? 1 : 0, idAnalise, idsProdutos);
            db.Database.ExecuteSqlCommand(sql);
        }

        void Item_Bound(Object sender, DataGridItemEventArgs e)
        {
            e.Item.Cells[4].Visible = false;
            if (e.Item.Cells[4] == null || e.Item.Cells[4].Text == "False")
                e.Item.ForeColor = System.Drawing.Color.FromName("Red");
            else
                e.Item.ForeColor = System.Drawing.Color.FromName("Black");
        }

        /// <summary>
        /// Gerar um arquivo apartir de um datatable
        /// </summary>
        /// <param name="data">Dados</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>Caminho do arquivo</returns>
        private string SalvarExcel(DataTable data, string nomeArquivo)
        {
            // create the DataGrid and perform the databinding
            System.Web.UI.WebControls.DataGrid grid =
                        new System.Web.UI.WebControls.DataGrid();

            grid.HeaderStyle.Font.Bold = true;
            grid.DataSource = data;
            grid.DataMember = data.TableName;
            grid.ItemDataBound += Item_Bound;

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

        private DataTable FormatarDataTable(DataTable dt, EnumTipoArquivo tipoArquivo)
        {
            switch (tipoArquivo)
            {
                case EnumTipoArquivo.ProdutoExcel:
                    dt.Columns["Id"].ColumnName = "Id";
                    dt.Columns["DescricaoOriginalProduto"].ColumnName = "Descrição Original";
                    dt.Columns["DescricaoUnificada"].ColumnName = "Descrição Unificada";
                    dt.Columns["Unidade"].ColumnName = "Unidade";
                    break;
            }

            return dt;
        }

    }
}
