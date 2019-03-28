using LaefazWeb.Enumerators;
using LaefazWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.Jenkins.PosBuild
{
    class Tosca
    {
       
        public void processa(int IdScriptCondicaoScript, int IdTestData)
        {
            DbEntities db = new DbEntities();

            //Simula os parâmetros de saída //////////////////////////
            List<ParametroScript> ListaParametroScript = db.ParametroScript.Where(x => x.IdScript_CondicaoScript == IdScriptCondicaoScript && x.IdTipoParametro == (int)EnumTipoParametro.Output).ToList();

            Dictionary<string, string> parametroSaida = new Dictionary<string, string>();
            ListaParametroScript.ForEach(ps =>
            {
                Random randNum = new Random();
                randNum.Next(4);
              
                string value = db.Parametro.Where(x => x.Id == ps.IdParametro).FirstOrDefault().Descricao + "_Teste_" + randNum.Next();

                ParametroValor pv = db.ParametroValor.Where(x=> x.IdParametroScript == ps.Id && x.IdTestData == IdTestData).FirstOrDefault();
                pv.Valor = value;

                // anexar objeto ao contexto
                db.ParametroValor.Attach(pv);
                //Prepara a entidade para uma Edição
                db.Entry(pv).State = System.Data.Entity.EntityState.Modified;
                // informa que o obejto será modificado
                db.SaveChanges();

            });
            /////////////////////////////////////////////////////////
        }



    }
}
