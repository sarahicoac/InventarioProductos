//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventarioProductos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TSucursal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TSucursal()
        {
            this.Suc_Pro = new HashSet<Suc_Pro>();
        }
    
        public int IdSuc { get; set; }
        public string NombreSuc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Suc_Pro> Suc_Pro { get; set; }
    }
}
