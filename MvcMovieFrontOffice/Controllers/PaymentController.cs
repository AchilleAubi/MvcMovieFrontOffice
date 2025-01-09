using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcMovieFrontOffice.Services;
using OfficeOpenXml;

namespace MvcMovieFrontOffice.Controllers;

[Authorize]
public class PaymentController(PaymentService paymentService) : Controller
{
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
    {
        var payments = await paymentService.GetPaymentByUserIdAsync(GetCurrentUserId(), startDate, endDate);
        
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
        
        return View(payments);
    }
    
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpPost]
    public async Task<IActionResult> ExportDataToExcel(DateTime? startDate, DateTime? endDate)
    {
        var payments = await paymentService.GetPaymentByUserIdAsync(GetCurrentUserId(), startDate, endDate);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Payments");

            worksheet.Cells["A1"].Value = "PAYMENT N°";
            worksheet.Cells["B1"].Value = "RESERVATION N°";
            worksheet.Cells["C1"].Value = "PAYMENT DATE";
            worksheet.Cells["D1"].Value = "PAYMENT METHOD";
            worksheet.Cells["E1"].Value = "AMOUNT (€)";

            for (int i = 0; i < payments.Count; i++)
            {
                var payment = payments[i];
                worksheet.Cells[i + 2, 1].Value = payment.Id;
                worksheet.Cells[i + 2, 2].Value = payment.ReservationId;
                worksheet.Cells[i + 2, 3].Value = payment.PaymentDate.ToString("dd/MM/yyyy");
                worksheet.Cells[i + 2, 4].Value = payment.PaymentMethod;
                worksheet.Cells[i + 2, 5].Value = payment.Amount;
            }
            
            var stream = new MemoryStream(package.GetAsByteArray());
            
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Payment.xlsx");
        }
    }
}