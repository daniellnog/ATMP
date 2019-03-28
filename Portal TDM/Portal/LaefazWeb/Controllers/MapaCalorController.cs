using LaefazWeb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TDMWeb.Extensions;
using WebGrease.Css.Extensions;
using TDMWeb.Lib;
using System.Linq.Dynamic;
using LaefazWeb.Models.VOs;
using Newtonsoft.Json;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class MapaCalorController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index(string wid, string hei)
        {
            ViewBag.width = wid;
            ViewBag.height = hei;
            ViewBag.res = wid + "x" + hei;
            return View(db.TelaMapaCalor.ToList());
        }

        public ActionResult TestData(string wid, string hei)
        {
            ViewBag.width = wid;
            ViewBag.height = hei;
            ViewBag.res = wid + "x" + hei;
            return View(db.TelaMapaCalor.ToList());
        }
        // Função para trazer as coordenadas da tabela do Mapa de Calor(Datapool)
        public JsonResult exibirMapaCalor(string dtInicio, string dtTermino, string res)
        {
            //convertendo de string para DateTime
            DateTime dataInicio = DateTime.Parse(dtInicio);
            DateTime dataTermino = DateTime.Parse(dtTermino);
            //Criando lista com as coordenadas do mapa de calor e o contador de clicks
            List<MapaCalorVO> listaMapaCalor =
                        (from m in db.MapaCalor
                        where m.Data >= dataInicio && m.Data <= dataTermino && m.IdTelaMapaCalor == 1 && m.Resolucao == res
                         group m by new
                        {
                            m.PosX,
                            m.PosY
                        } into grp
                        select new MapaCalorVO
                        {
                            Count = grp.Count(),
                            PosX = grp.Key.PosX,
                            PosY = grp.Key.PosY
                        }).ToList();

            string json = JsonConvert.SerializeObject(listaMapaCalor, Formatting.Indented);

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        // Função para trazer as coordenadas da tabela do Mapa de Calor(TestData)
        public JsonResult exibirMapaCalorTestData(string dtInicio, string dtTermino, string res)
        {
            //convertendo de string para DateTime
            DateTime dataInicio = DateTime.Parse(dtInicio);
            DateTime dataTermino = DateTime.Parse(dtTermino);
            //Criando lista com as coordenadas do mapa de calor e o contador de clicks
            List<MapaCalorVO> listaMapaCalor =
                        (from m in db.MapaCalor
                         where m.Data >= dataInicio && m.Data <= dataTermino && m.IdTelaMapaCalor == 2 && m.Resolucao == res
                         group m by new
                         {
                             m.PosX,
                             m.PosY
                         } into grp
                         select new MapaCalorVO
                         {
                             Count = grp.Count(),
                             PosX = grp.Key.PosX,
                             PosY = grp.Key.PosY
                         }).ToList();

            string json = JsonConvert.SerializeObject(listaMapaCalor, Formatting.Indented);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }

    
}