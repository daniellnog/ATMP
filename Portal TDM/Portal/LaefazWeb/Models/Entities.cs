using LaefazWeb.Models.VOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace LaefazWeb.Models
{
    public class Entities : DbEntities
    {

        public Entities() 
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbEntities>());
        }

        //public override int SaveChanges()
        //{
        //    // Detecta as alterações existentes na instância corrente do DbContext.
        //    this.ChangeTracker.DetectChanges();

           
        //    var entries = DetectEntries();                  

        //    // Cria lista para armazenamento temporário dos registros em Historico.
        //        List<Historico> Historicos = new List<Historico>();

        //    // Varre as entidades que devem gerar registros em Historico.
        //    foreach (var entry in entries)
        //    {
        //        // Cria novo registro de Historico.
        //        //Historico newHistorico = GetHistorico(entry);

        //        //if (newHistorico != null)
        //        //    Historicos.Add(newHistorico);
        //    }
        //    // Adiciona os registros de Historico na fonte de dados.
        //    foreach (var item in Historicos)
        //    {
        //        this.Entry(item).State = EntityState.Added;

        //    }


        //    return base.SaveChanges();

        //}


        /// <summary>
        /// Identifica quais entidades devem ser gerar registros de Historico.
        /// </summary>
        private IEnumerable<DbEntityEntry> DetectEntries()
        {
            return ChangeTracker.Entries().Where(e => (e.State == EntityState.Deleted ||
                                                       e.State == EntityState.Modified ||
                                                       e.State == EntityState.Added) &&
                                                       e.Entity.GetType().Name.Contains("TestData") ||
                                                       e.Entity.GetType().Name.Contains("DataPool") || 
                                                       e.Entity.GetType().Name.Contains("Execucao") 
                                                       );





        }

        /// <summary>
        /// Cria os registros de Historico.
        /// </summary>
        //private Historico GetHistorico(DbEntityEntry entry)
        //{

        //    Historico returnValue = null;

        //    if (entry.State == EntityState.Added)
        //    {
        //        returnValue = GetInsertHistorico(entry);
        //    }
        //    else if (entry.State == EntityState.Modified)
        //    {
        //        returnValue = GetUpdateHistorico(entry);
        //    }
        //    else if (entry.State == EntityState.Deleted)
        //    {
        //        returnValue = GetDeleteHistorico(entry);
        //    }

        //    return returnValue;
        //}

        //private Historico GetInsertHistorico(DbEntityEntry entry)
        //{

        //    //return HistoricoVO.CreateInsertHistorico(entry.Entity);
        //}

        private Historico GetDeleteHistorico(DbEntityEntry entry)
        {

            return HistoricoVO.CreateDeleteHistorico(entry.Entity);
        }

        private Historico GetUpdateHistorico(DbEntityEntry entry)
        {

            object originalValue = null;

            //if (entry.OriginalValues != null)
            //    originalValue = entry.OriginalValues.ToObject();
            //else
            //    originalValue = entry.GetDatabaseValues().ToObject();

            return HistoricoVO.CreateUpdateHistorico(entry.OriginalValues.ToObject(), entry.CurrentValues.ToObject());
        }

    }
}