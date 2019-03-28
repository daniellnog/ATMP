﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaefazWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DbEntities : DbContext
    {
        public DbEntities()
            : base("name=DbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MapaCalor> MapaCalor { get; set; }
        public virtual DbSet<StatusExecucao> StatusExecucao { get; set; }
        public virtual DbSet<TelaMapaCalor> TelaMapaCalor { get; set; }
        public virtual DbSet<Agendamento> Agendamento { get; set; }
        public virtual DbSet<Agendamento_TestData> Agendamento_TestData { get; set; }
        public virtual DbSet<AmbienteExecucao> AmbienteExecucao { get; set; }
        public virtual DbSet<AmbienteVirtual> AmbienteVirtual { get; set; }
        public virtual DbSet<AUT> AUT { get; set; }
        public virtual DbSet<CondicaoScript> CondicaoScript { get; set; }
        public virtual DbSet<CT> CT { get; set; }
        public virtual DbSet<DataPool> DataPool { get; set; }
        public virtual DbSet<Demanda> Demanda { get; set; }
        public virtual DbSet<DeParaAtmpAlmFase> DeParaAtmpAlmFase { get; set; }
        public virtual DbSet<DeParaAtmpAlmSistema> DeParaAtmpAlmSistema { get; set; }
        public virtual DbSet<Encadeamento> Encadeamento { get; set; }
        public virtual DbSet<Encadeamento_TestData> Encadeamento_TestData { get; set; }
        public virtual DbSet<Execucao> Execucao { get; set; }
        public virtual DbSet<Historico> Historico { get; set; }
        public virtual DbSet<Parametro> Parametro { get; set; }
        public virtual DbSet<ParametroScript> ParametroScript { get; set; }
        public virtual DbSet<ParametroScript_Valor> ParametroScript_Valor { get; set; }
        public virtual DbSet<ParametroValor> ParametroValor { get; set; }
        public virtual DbSet<ParametroValor_Historico> ParametroValor_Historico { get; set; }
        public virtual DbSet<PlanoTeste_TI> PlanoTeste_TI { get; set; }
        public virtual DbSet<PlanoTeste_TI_Historico> PlanoTeste_TI_Historico { get; set; }
        public virtual DbSet<Plataforma> Plataforma { get; set; }
        public virtual DbSet<Script> Script { get; set; }
        public virtual DbSet<Script_CondicaoScript> Script_CondicaoScript { get; set; }
        public virtual DbSet<Script_CondicaoScript_Ambiente> Script_CondicaoScript_Ambiente { get; set; }
        public virtual DbSet<Script_CondicaoScript_CT> Script_CondicaoScript_CT { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TDM> TDM { get; set; }
        public virtual DbSet<TDM_Usuario> TDM_Usuario { get; set; }
        public virtual DbSet<TestData> TestData { get; set; }
        public virtual DbSet<TestData_CT> TestData_CT { get; set; }
        public virtual DbSet<TipoDadoParametro> TipoDadoParametro { get; set; }
        public virtual DbSet<TipoFaseTeste> TipoFaseTeste { get; set; }
        public virtual DbSet<TipoParametro> TipoParametro { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<MotivoExecucao> MotivoExecucao { get; set; }
    
        public virtual ObjectResult<PR_LISTAR_DATAPOOL_Result> PR_LISTAR_DATAPOOL(Nullable<int> dISPLAYLENGTH, Nullable<int> dISPLAYSTART, Nullable<int> sORTCOL, string sORTDIR, string sEARCH, Nullable<int> lISTARTODOS, Nullable<int> iDUSUARIO, string iDTDM)
        {
            var dISPLAYLENGTHParameter = dISPLAYLENGTH.HasValue ?
                new ObjectParameter("DISPLAYLENGTH", dISPLAYLENGTH) :
                new ObjectParameter("DISPLAYLENGTH", typeof(int));
    
            var dISPLAYSTARTParameter = dISPLAYSTART.HasValue ?
                new ObjectParameter("DISPLAYSTART", dISPLAYSTART) :
                new ObjectParameter("DISPLAYSTART", typeof(int));
    
            var sORTCOLParameter = sORTCOL.HasValue ?
                new ObjectParameter("SORTCOL", sORTCOL) :
                new ObjectParameter("SORTCOL", typeof(int));
    
            var sORTDIRParameter = sORTDIR != null ?
                new ObjectParameter("SORTDIR", sORTDIR) :
                new ObjectParameter("SORTDIR", typeof(string));
    
            var sEARCHParameter = sEARCH != null ?
                new ObjectParameter("SEARCH", sEARCH) :
                new ObjectParameter("SEARCH", typeof(string));
    
            var lISTARTODOSParameter = lISTARTODOS.HasValue ?
                new ObjectParameter("LISTARTODOS", lISTARTODOS) :
                new ObjectParameter("LISTARTODOS", typeof(int));
    
            var iDUSUARIOParameter = iDUSUARIO.HasValue ?
                new ObjectParameter("IDUSUARIO", iDUSUARIO) :
                new ObjectParameter("IDUSUARIO", typeof(int));
    
            var iDTDMParameter = iDTDM != null ?
                new ObjectParameter("IDTDM", iDTDM) :
                new ObjectParameter("IDTDM", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_LISTAR_DATAPOOL_Result>("PR_LISTAR_DATAPOOL", dISPLAYLENGTHParameter, dISPLAYSTARTParameter, sORTCOLParameter, sORTDIRParameter, sEARCHParameter, lISTARTODOSParameter, iDUSUARIOParameter, iDTDMParameter);
        }
    
        public virtual ObjectResult<PR_LISTAR_EXECUCAO_Result> PR_LISTAR_EXECUCAO(Nullable<int> dISPLAYLENGTH, Nullable<int> dISPLAYSTART, Nullable<int> sORTCOL, string sORTDIR, string sEARCH, Nullable<int> lISTARTODOS)
        {
            var dISPLAYLENGTHParameter = dISPLAYLENGTH.HasValue ?
                new ObjectParameter("DISPLAYLENGTH", dISPLAYLENGTH) :
                new ObjectParameter("DISPLAYLENGTH", typeof(int));
    
            var dISPLAYSTARTParameter = dISPLAYSTART.HasValue ?
                new ObjectParameter("DISPLAYSTART", dISPLAYSTART) :
                new ObjectParameter("DISPLAYSTART", typeof(int));
    
            var sORTCOLParameter = sORTCOL.HasValue ?
                new ObjectParameter("SORTCOL", sORTCOL) :
                new ObjectParameter("SORTCOL", typeof(int));
    
            var sORTDIRParameter = sORTDIR != null ?
                new ObjectParameter("SORTDIR", sORTDIR) :
                new ObjectParameter("SORTDIR", typeof(string));
    
            var sEARCHParameter = sEARCH != null ?
                new ObjectParameter("SEARCH", sEARCH) :
                new ObjectParameter("SEARCH", typeof(string));
    
            var lISTARTODOSParameter = lISTARTODOS.HasValue ?
                new ObjectParameter("LISTARTODOS", lISTARTODOS) :
                new ObjectParameter("LISTARTODOS", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_LISTAR_EXECUCAO_Result>("PR_LISTAR_EXECUCAO", dISPLAYLENGTHParameter, dISPLAYSTARTParameter, sORTCOLParameter, sORTDIRParameter, sEARCHParameter, lISTARTODOSParameter);
        }
    }
}
