using Plataforma.Areas.PCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plataforma.Areas.PCD.Models
{
    public class DocumentosCurso
    {
        public curso curso { get; set; }
        public ICollection<nivele> niveles { get; set; }
        public ICollection<documento> documentos { get; set; }

        public ICollection<int> documentosSeleccionados { get; set; }

        public int unidad { get; set; }

        public int capitulo { get; set; }
    }
}