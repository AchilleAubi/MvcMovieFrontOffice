using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MvcMovieFrontOffice.Models;
using PdfSharp.Pdf;

namespace MvcMovieFrontOffice.Services;

public class InvoiceService
{
    public PdfDocument GetInvoicePdf(Reservation reservation)
    {
        var pdfDocument = new Document();
        
        BuildDocument(pdfDocument, reservation);

        var pdfRender = new PdfDocumentRenderer();
        pdfRender.Document = pdfDocument;
        
        pdfRender.RenderDocument();
        
        return pdfRender.PdfDocument;
    }

    private static void BuildDocument(Document pdfDocument, Reservation reservation)
    {
        var section = pdfDocument.AddSection();
        
        var title = section.AddParagraph();
        title.AddText("Invoice Details");
        title.Format.Font.Size = 16;
        title.Format.Font.Bold = true;
        title.Format.Alignment = ParagraphAlignment.Center;
        title.Format.SpaceAfter = 20;
        
        var table = section.AddTable();
        table.Borders.Width = 0.75;
        
        table.AddColumn(Unit.FromCentimeter(5));
        table.AddColumn(Unit.FromCentimeter(10));
       
        AddRow(table, "Invoice Name", "Reservation");
        AddRow(table, "Vehicle Matriculation", reservation.VehicleId.ToString());
        AddRow(table, "User Matriculation", reservation.UserId);
        AddRow(table, "Start Date", reservation.StartDate.ToString("dd/MM/yyyy"));
        AddRow(table, "End Date", reservation.EndDate.ToString("dd/MM/yyyy"));
        AddRow(table, "Status", reservation.Status);
        AddRow(table, "Total Amount", reservation.TotalPrice.ToString("C"));
        
        section.AddParagraph().Format.SpaceAfter = 20;
    }
    
    private static void AddRow(Table table, string label, string value)
    {
        var row = table.AddRow();
        row.Cells[0].AddParagraph(label);
        row.Cells[0].Format.Font.Bold = true;
        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

        row.Cells[1].AddParagraph(value);
        row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
    }

}