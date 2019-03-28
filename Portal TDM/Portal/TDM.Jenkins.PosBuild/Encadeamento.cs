using LaefazWeb.Enumerators;
using LaefazWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.Jenkins.PosBuild
{
    class Encadeamento
    {
        private List<ParametroScript> ListaParametroScript;
        private Dictionary<string, string> DicionarioParametros;
        private DbEntities db;

        public Encadeamento()
        {
            this.db = new DbEntities();
            this.DicionarioParametros = new Dictionary<string, string>();
        }

        #region GetDicionarioParametros()
        public Dictionary<string, string> GetParametrosEntrada()
        {
            return this.DicionarioParametros;
        }
        #endregion

        #region GetListaParametroScript()
        public List<ParametroScript> GetListaParametroScript()
        {
            return this.ListaParametroScript;
        }
        #endregion

        public void InterromperEncadeamento(TestData td)
        {
            throw new EncadeamentoException("O Encadeamento foi interrompido pois uma das execuções da cadeia falhou.", new Exception("O TestData de Id "+td.Id+" associado ao Encadeamento '"+td.Encadeamento_TestData.FirstOrDefault().Encadeamento.Descricao+"' de Id "+ td.Encadeamento_TestData.FirstOrDefault().IdEncadeamento));
        }

        public void CarregarParametosEntrada(int IdTestData, int IdScript_CondicaoScript)
        {

            if (DicionarioParametros.Count() > 0)
            {
                db = new DbEntities();
                this.ListaParametroScript = db.ParametroScript.Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript && x.IdTipoParametro == (int)EnumTipoParametro.Input).ToList();


                ListaParametroScript.ForEach(x =>
                {
                    Parametro par = db.Parametro.Find(x.IdParametro);
                    if (this.DicionarioParametros.ContainsKey(par.Descricao))
                    {
                        ParametroValor pv = db.ParametroValor.Where(y => y.IdParametroScript == x.Id && y.IdTestData == IdTestData).FirstOrDefault();
                        pv.Valor = this.DicionarioParametros[par.Descricao];

                        db.ParametroValor.Attach(pv);
                        //Prepara a entidade para uma Edição
                        db.Entry(pv).State = System.Data.Entity.EntityState.Modified;
                        // informa que o obejto será modificado
                        db.SaveChanges();
                    }

                });
            }
           
        }

        public void CarregarParametrosSaida(int IdTestData, int IdScript_CondicaoScript)
        {
            db = new DbEntities();
            this.ListaParametroScript = db.ParametroScript.Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript && x.IdTipoParametro == (int)EnumTipoParametro.Output).ToList();


            ListaParametroScript.ForEach(ps =>
            {

                ParametroValor pv = db.ParametroValor.Where(x => x.IdParametroScript == ps.Id && x.IdTestData == IdTestData).FirstOrDefault();
                string key = db.Parametro.Where(x => x.Id == ps.IdParametro).FirstOrDefault().Descricao;
                string valor = pv.Valor;

                if (DicionarioParametros.ContainsKey(key))
                {
                    DicionarioParametros[key] = valor;
                }
                else
                {
                    DicionarioParametros.Add(key, valor);
                }

            });

        }

    }
}
