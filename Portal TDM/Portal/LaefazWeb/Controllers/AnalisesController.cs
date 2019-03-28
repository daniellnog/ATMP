using LaefazWeb.Models;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using TDMWeb.Extensions;
//using TDMWeb.Enumerators;
using TDMWeb.Lib;


namespace TDMWeb.Controllers
{
	[UsuarioLogado]
	public class AnalisesController : Controller
	{
		private DbEntities db = new DbEntities();


		//
		// GET: /Analises/

		public ActionResult Index()
		{
			return View(/*db.Analise.ToList()*/);
		}

		//
		// GET: /Analises/Adicionar

		public ActionResult Adicionar()
		{
			/*ViewBag.ListaSistema = db.Sistema.ToList();
			ViewBag.ListaTipoMassa = db.TipoMassa.ToList();*/

			return View();
		}

		[HttpPost]
		public ActionResult Remover(int idAnalise)
		{
			var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

			try
			{
				db.Database.CommandTimeout = 180;
				DeletarAnalise(idAnalise);
				//DeletarEstoque(idAnalise, "I");
				//DeletarEstoque(idAnalise, "F");
				//DeletarMovimentacao(idAnalise, "E");
				//DeletarMovimentacao(idAnalise, "S");

				//Analise analise = db.Analises.SingleOrDefault(a => a.Id == idAnalise);

				//db.Analises.Remove(analise);
				//db.SaveChanges();

				result.Data = new { Result = "Análise removida com sucesso.", Status = (int)WebExceptionStatus.Success };
			}
			catch (Exception ex)
			{
				result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
			}
			return result;
		}

		private void DeletarAnalise(int idAnalise)
		{
			SqlParameter[] parametros = { new SqlParameter("@IdAnalise", idAnalise) };

			db.Database.CommandTimeout = 180;
			db.Database.ExecuteSqlCommand("EXEC PR_EXCLUIR_ANALISE @IdAnalise", parametros);
		}

		public ActionResult Editar(int idAnalise)
		{
			return View(/*db.Analise.FirstOrDefault(a => a.Id == idAnalise)*/);
		}

		public ActionResult SalvarEdicao(/*Analise analise*/)
		{
			try
			{
				//Salvar(analise, true);
				this.FlashSuccess("Análise editada com sucesso.");
			}
			catch (Exception ex)
			{
				this.FlashError(ex.Message);
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Salvar(/*Analise Analise, bool editar = false*/)
		{
			/*
			try
			{
				if (!ModelState.IsValid)
				{
					var msg = string.Empty;
					ModelState.Values.SelectMany(v => v.Errors).ForEach(m => msg = string.Concat(m.ErrorMessage.ToString(), @"\n"));
					if (!msg.IsNullOrWhiteSpace())
						this.FlashWarning(msg);
					ViewBag.ListaAnalises = db.Analise.ToList();
					return View("Adicionar", Analise);
				}

				Analise analise;

				if (editar)
				{
					analise = Analise;
				}
				else
				{
					analise = new Analise()
					{
						IdSistema = Int32.Parse(Request.Form.Get("analise_sistema")),
						IdTipoMassa = Int32.Parse(Request.Form.Get("analise_tipo_massa")),
						Descricao = Request.Form.Get("analise_observacoes"),
						QtdSolicitada = Int32.Parse(Request.Form.Get("analise_qtd_solicitada")),
					};

					db.Analise.Add(analise);
					db.SaveChanges();

					this.FlashSuccess("Análise adicionada.");
				}
				
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Analise_Descricao"))
					this.FlashError("Já existe uma análise com essa descrição.");
				else
					this.FlashError(ex.Message);
			}
			*/
			return RedirectToAction("Index");
		}

		
		private int RemoverProtuto(int idAnalise)
		{
			SqlParameter[] param =
			{
				new SqlParameter("@IdAnalise", idAnalise)
			};

			var retorno = db.Database.ExecuteSqlCommand("EXEC PR_REMOVER_PRODUTOS @IdAnalise", param);

			return retorno;
		}
	}



}
