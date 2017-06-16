using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Plataforma.Areas.PCD.Controllers
{
    public partial class AdministrarReportes : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
    {
        Paragraph footer = new Paragraph("Publicaciones Innovadoras en Matemática para Secundaria PIMAS 	 Cédula Jurídica: 3-101-469172" +
                                         "\n             editorial @pimas.co.cr ⧫ www.pimas.co.cr ⧫ Facebook / PimasCR ⧫ Tel: 8310 0573",
                                          FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLDITALIC));

        footer.Alignment = Element.ALIGN_CENTER;

        PdfPTable footerTbl = new PdfPTable(1);

        footerTbl.TotalWidth = 400;

        footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;


        PdfPCell cell = new PdfPCell(footer);

        cell.Border = 0;

        cell.PaddingLeft = 10;

        footerTbl.AddCell(cell);

        footerTbl.WriteSelectedRows(0, -1, 95, 30, writer.DirectContent);

    }
}
}