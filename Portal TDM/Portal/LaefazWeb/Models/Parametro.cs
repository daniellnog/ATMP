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
    
    public partial class Parametro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parametro()
        {
            this.ParametroScript = new HashSet<ParametroScript>();
        }
    
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string ColunaTecnicaTosca { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParametroScript> ParametroScript { get; set; }
    }
}
