//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Plataforma.Areas.PCD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class venta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public venta()
        {
            this.ventas_articulos = new HashSet<ventas_articulos>();
        }
    
        public int id { get; set; }
        public string nombre_comprador { get; set; }
        public string nombre_receptor { get; set; }
        public string numero_comprador { get; set; }
        public string numero_receptor { get; set; }
        public string direccion { get; set; }
        public string horario_preferencia { get; set; }
        public Nullable<System.DateTime> tiempo_estimado { get; set; }
        public int id_transporte { get; set; }
        public int id_tipo_pago { get; set; }
        public int id_usuario { get; set; }
        public int id_estado_venta { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<int> id_transaccion { get; set; }
    
        public virtual usuario usuario { get; set; }
        public virtual estado_venta estado_venta { get; set; }
        public virtual tipos_pago tipos_pago { get; set; }
        public virtual transporte transporte { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ventas_articulos> ventas_articulos { get; set; }
    }
}
