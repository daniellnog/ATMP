//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Script_CondicaoScript_Ambiente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Script_CondicaoScript_Ambiente()
        {
            this.Execucao = new HashSet<Execucao>();
        }
    
        public int Id { get; set; }
        public int IdScript_CondicaoScript { get; set; }
        public Nullable<int> IdAmbienteVirtual { get; set; }
        public Nullable<int> IdAmbienteExecucao { get; set; }
    
        public virtual AmbienteExecucao AmbienteExecucao { get; set; }
        public virtual AmbienteVirtual AmbienteVirtual { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Execucao> Execucao { get; set; }
        public virtual Script_CondicaoScript Script_CondicaoScript { get; set; }
    }
}
