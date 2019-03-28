using LaefazWeb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TDMWeb.Extensions;
using TDMWeb.Lib;
using WebGrease.Css.Extensions;
using TDMWeb.Models.VOs;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class TDMController : Controller
    {
        private Usuario user;
        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            this.user = (Usuario)Session["ObjUsuario"];

            List<TDM_Usuario> listaTDMByUser = new List<TDM_Usuario>();
            listaTDMByUser.AddRange(db.TDM_Usuario.Where(x => x.IdUsuario == user.Id).ToList());

            List<TDM> listaTDM = new List<TDM>();

            foreach (TDM_Usuario tdmUsuario in listaTDMByUser)
            {
                listaTDM.AddRange(db.TDM.Where(x => x.Id == tdmUsuario.IdTDM).ToList());
            }

            return View(listaTDM);
        }

        public ActionResult Adicionar()
        {
            this.user = (Usuario)Session["ObjUsuario"];
            List<ChaveVO> lista = new List<ChaveVO>();

            lista.Add(new ChaveVO("Não", 0));
            lista.Add(new ChaveVO("Sim", 1));

            ViewBag.listaTdmPublico = lista;
            return View();
        }

        [HttpPost]
        public ActionResult Remover(int id)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            TDM tdm = db.TDM.Find(id);
            this.user = (Usuario)Session["ObjUsuario"];

            if (tdm.TdmPublico)
            {
                result.Data = new { Result = "Não é possível excluir um TDM Público", Status = (int)WebExceptionStatus.UnknownError };
            }

            try
            {

                List<DataPool> dataPools = db.DataPool.Where(x => x.IdTDM == id).ToList();

                if (dataPools.Count() > 0)
                    throw new Exception("O TDM possui relacionamento com Datapools.", new Exception("FK_DataPool_TDM"));

                TDM_Usuario tdm_usuario = db.TDM_Usuario.Where(x => x.IdTDM == id).Where(x => x.IdUsuario == user.Id).FirstOrDefault();
                db.TDM_Usuario.Remove(tdm_usuario);
                db.SaveChanges();
                db.TDM.Remove(tdm);
                db.SaveChanges();

                result.Data = new { Result = "TDM removida com sucesso.", Status = (int)WebExceptionStatus.Success };
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException != null && ex.InnerException.Message.ToString().Contains("FK_DataPool_TDM"))

                    result.Data = new { Result = "Esse registro contém dependência com outra entidade.", Status = (int)WebExceptionStatus.SendFailure };
                else
                    result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public ActionResult Editar(int id)
        {
            //List<ChaveVO> lista = new List<ChaveVO>();

            //lista.Add(new ChaveVO("Não", 0));
            //lista.Add(new ChaveVO("Sim", 1));

            //ViewBag.listaTdmPublico = lista;

            return View(db.TDM.FirstOrDefault(a => a.Id == id));
        }

        public ActionResult SalvarEdicao(TDM objeto)
        {
            try
            {
                Salvar(objeto, true);
                //this.FlashSuccess("TDM editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Salvar(TDM objeto, bool editar = false)
        {
            this.user = (Usuario)Session["ObjUsuario"];
            try
            {
                if (!ModelState.IsValid)
                {
                    var msg = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ForEach(m => msg = string.Concat(m.ErrorMessage.ToString(), @"\n"));
                    if (!msg.IsNullOrWhiteSpace())
                        this.FlashWarning(msg);

                    return View("Adicionar", objeto);
                }

                TDM tdm;

                if (editar)
                {
                    //bool bOption = false;

                    //if (Request.Form.Get("listTdmPublico").Equals("1"))
                    //{
                    //    bOption = true;
                    //}

                    //objeto.TdmPublico = bOption;
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //TDM_Usuario tdmUsuario = new TDM_Usuario()
                    //{
                    //    IdTDM = objeto.Id,
                    //    IdUsuario = Util.GetUsuarioLogado().Id
                    //};

                    //db.TDM_Usuario.Add(tdmUsuario);
                    //db.SaveChanges();

                    this.FlashSuccess("TDM editado com sucesso!");
                }
                else
                {
                    //bool bOption = false;

                    //if (Request.Form.Get("listTdmPublico").Equals("1")){
                    //    bOption = true;
                    //}

                    tdm = new TDM()
                    {
                        Descricao = Request.Form.Get("tdm_descricao"),
                        TdmPublico = false,
                    };

                    // Criando o objeto TDM Usuario e realizando a inserção no banco com os dados do Usuário responsável pela criação do TDM.
                    TDM_Usuario tdm_usuario = new TDM_Usuario();
                    tdm_usuario.IdTDM = tdm.Id;
                    tdm_usuario.IdUsuario = this.user.Id;

                    // Salvando as alterações no banco
                    db.TDM.Add(tdm);
                    db.TDM_Usuario.Add(tdm_usuario);
                    db.SaveChanges();

                    this.FlashSuccess("TDM adicionado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_TDM_Descricao"))
                    this.FlashWarning("Já existe um TDM com essa descrição. Para ter acesso contacte o administrador do sistema.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }


        public JsonResult GetTDMPublico(string id)
        {

            int IdTDM = int.Parse(id);
            TDM tdm;
            if (IdTDM == 0)
                tdm = new TDM()
                {
                    Descricao = "TODOS",
                    Id = 0,
                    TdmPublico = false
                };
            else
                tdm = db.TDM.Find(IdTDM);

            int IsPublic = 0;
            if (tdm.TdmPublico)
            {
                IsPublic = 1;
            }

            return (Json(new { tdmPublico = IsPublic, Id = tdm.Id }, JsonRequestBehavior.AllowGet));

        }


        public JsonResult GetTDMByIdDemanda(string id)
        {
            int idTestData = Int32.Parse(id);
           
            TestData testData = db.TestData.FirstOrDefault(x => x.Id == idTestData);
            int IdTDM = testData.DataPool.IdTDM;


            return (Json(new { TDM = IdTDM, TestData = testData.Descricao }, JsonRequestBehavior.AllowGet));
        }
    }
}
