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
    
    public partial class carrito
    {
        public int id_usuario { get; set; }
        public int id_articulo { get; set; }
        public int cantidad { get; set; }
    
        public virtual usuario usuario { get; set; }
        public virtual articulo articulo { get; set; }
    }
}
