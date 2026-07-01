using ClosedXML.Excel;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Infrastructure.Services;

public class ExcelReportService : IExcelReportService
{
    public byte[] GenerateAppointmentsReport(IEnumerable<AppointmentReportRow> data)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Citas");

        // Título
        sheet.Cell(1, 1).Value = "REPORTE DE CITAS - VetClinic Pro";
        sheet.Range(1, 1, 1, 7).Merge();
        sheet.Cell(1, 1).Style
            .Font.SetBold(true)
            .Font.SetFontSize(14)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Fill.SetBackgroundColor(XLColor.FromHtml("#2E75B6"))
            .Font.SetFontColor(XLColor.White);

        // Fecha generación
        sheet.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        sheet.Range(2, 1, 2, 7).Merge();
        sheet.Cell(2, 1).Style
            .Font.SetItalic(true)
            .Font.SetFontColor(XLColor.Gray);

        // Encabezados
        var headers = new[]
        {
            "Mascota", "Especialista", "Área de Servicio",
            "Fecha", "Hora Inicio", "Hora Fin", "Estado"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = sheet.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style
                .Font.SetBold(true)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#4472C4"))
                .Font.SetFontColor(XLColor.White)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        }

        // Datos
        int row = 5;
        foreach (var item in data)
        {
            sheet.Cell(row, 1).Value = item.PetName;
            sheet.Cell(row, 2).Value = item.SpecialistName;
            sheet.Cell(row, 3).Value = item.ServiceAreaName;
            sheet.Cell(row, 4).Value = item.AppointmentDate.ToString("dd/MM/yyyy");
            sheet.Cell(row, 5).Value = item.StartTime.ToString("HH:mm");
            sheet.Cell(row, 6).Value = item.EndTime.ToString("HH:mm");
            sheet.Cell(row, 7).Value = item.Status ?? "Sin estado";

            // Color por estado
            var statusCell = sheet.Cell(row, 7);
            statusCell.Style.Fill.SetBackgroundColor(item.Status switch
            {
                "Pendiente"  => XLColor.FromHtml("#FFF2CC"),
                "Completada" => XLColor.FromHtml("#E2EFDA"),
                "Cancelada"  => XLColor.FromHtml("#FCE4D6"),
                _            => XLColor.White
            });

            // Fila alternada
            if (row % 2 == 0)
            {
                for (int col = 1; col <= 6; col++)
                    sheet.Cell(row, col).Style
                        .Fill.SetBackgroundColor(XLColor.FromHtml("#F2F2F2"));
            }

            // Bordes fila
            sheet.Range(row, 1, row, 7).Style
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Hair);

            row++;
        }

        // Total
        sheet.Cell(row + 1, 1).Value = $"Total de citas: {data.Count()}";
        sheet.Cell(row + 1, 1).Style.Font.SetBold(true);

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] GenerateVaccinationsReport(IEnumerable<VaccinationReportRow> data)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Vacunaciones");
 
    // Colores de estado
    var colorVencida = XLColor.FromHtml("#FCE4D6");
    var colorProxima = XLColor.FromHtml("#FFF2CC");
    var colorAlDia = XLColor.FromHtml("#E2EFDA");
    var colorSinRefuerzo = XLColor.FromHtml("#F2F2F2");
 
    var fontVencida = XLColor.FromHtml("#C0392B");
    var fontProxima = XLColor.FromHtml("#B7950B");
    var fontAlDia = XLColor.FromHtml("#1E8449");
 
    // Título
    sheet.Cell(1, 1).Value = "REPORTE DE VACUNACIONES - VetClinic Pro";
    sheet.Range(1, 1, 1, 8).Merge();
    sheet.Cell(1, 1).Style
        .Font.SetBold(true)
        .Font.SetFontSize(15)
        .Font.SetFontName("Calibri")
        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        .Fill.SetBackgroundColor(XLColor.FromHtml("#375623"))
        .Font.SetFontColor(XLColor.White);
    sheet.Row(1).Height = 26;
 
    // Fecha generación
    sheet.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
    sheet.Range(2, 1, 2, 8).Merge();
    sheet.Cell(2, 1).Style
        .Font.SetItalic(true)
        .Font.SetFontColor(XLColor.Gray)
        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
 
    // ---- Resumen (dashboard mini) ----
    var list = data.ToList();
    int totalVencidas = list.Count(x => x.Status == "Vencida");
    int totalProximas = list.Count(x => x.Status == "Próxima");
    int totalAlDia = list.Count(x => x.Status == "Al día");
 
    sheet.Cell(4, 1).Value = "Vencidas";
    sheet.Cell(4, 2).Value = totalVencidas;
    sheet.Range(4, 1, 4, 2).Style.Fill.SetBackgroundColor(colorVencida);
    sheet.Cell(4, 1).Style.Font.SetBold(true).Font.SetFontColor(fontVencida);
    sheet.Cell(4, 2).Style.Font.SetBold(true).Font.SetFontColor(fontVencida);
 
    sheet.Cell(4, 4).Value = "Próximas (30 días)";
    sheet.Cell(4, 5).Value = totalProximas;
    sheet.Range(4, 4, 4, 5).Style.Fill.SetBackgroundColor(colorProxima);
    sheet.Cell(4, 4).Style.Font.SetBold(true).Font.SetFontColor(fontProxima);
    sheet.Cell(4, 5).Style.Font.SetBold(true).Font.SetFontColor(fontProxima);
 
    sheet.Cell(4, 7).Value = "Al día";
    sheet.Cell(4, 8).Value = totalAlDia;
    sheet.Range(4, 7, 4, 8).Style.Fill.SetBackgroundColor(colorAlDia);
    sheet.Cell(4, 7).Style.Font.SetBold(true).Font.SetFontColor(fontAlDia);
    sheet.Cell(4, 8).Style.Font.SetBold(true).Font.SetFontColor(fontAlDia);
 
    for (int r = 4; r <= 4; r++)
        for (int c = 1; c <= 8; c++)
            sheet.Cell(r, c).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
 
    // Encabezados
    var headers = new[]
    {
        "Mascota", "Vacuna", "Veterinario", "Lote",
        "Fecha Aplicación", "Próximo Refuerzo", "Días Vencida", "Estado"
    };
 
    int headerRow = 6;
    for (int i = 0; i < headers.Length; i++)
    {
        var cell = sheet.Cell(headerRow, i + 1);
        cell.Value = headers[i];
        cell.Style
            .Font.SetBold(true)
            .Fill.SetBackgroundColor(XLColor.FromHtml("#548235"))
            .Font.SetFontColor(XLColor.White)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
    }
    sheet.Row(headerRow).Height = 20;
    sheet.SheetView.FreezeRows(headerRow);
 
    // Datos
    int row = headerRow + 1;
    foreach (var item in list)
    {
        sheet.Cell(row, 1).Value = item.PetName;
        sheet.Cell(row, 2).Value = item.VaccineName;
        sheet.Cell(row, 3).Value = item.VeterinarianName;
        sheet.Cell(row, 4).Value = item.BatchNumber ?? "N/A";
        sheet.Cell(row, 5).Value = item.ApplicationDate?.ToString("dd/MM/yyyy") ?? "N/A";
        sheet.Cell(row, 6).Value = item.NextBoosterDate?.ToString("dd/MM/yyyy") ?? "N/A";
        if (item.DaysOverdue > 0)
            sheet.Cell(row, 7).Value = item.DaysOverdue;
        else
            sheet.Cell(row, 7).Value = "-";
 
        sheet.Cell(row, 8).Value = item.Status;
 
        var (fill, font) = item.Status switch
        {
            "Vencida" => (colorVencida, fontVencida),
            "Próxima" => (colorProxima, fontProxima),
            "Al día" => (colorAlDia, fontAlDia),
            _ => (colorSinRefuerzo, XLColor.Gray)
        };
 
        sheet.Range(row, 7, row, 8).Style
            .Fill.SetBackgroundColor(fill)
            .Font.SetFontColor(font)
            .Font.SetBold(true)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
 
        // Fila alternada solo en columnas sin color de estado
        if (row % 2 == 0)
            for (int col = 1; col <= 6; col++)
                sheet.Cell(row, col).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F7F7F7"));
 
        sheet.Range(row, 1, row, 8).Style
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            .Border.SetInsideBorder(XLBorderStyleValues.Hair);
 
        row++;
    }
 
    // Total
    row += 1;
    sheet.Cell(row, 1).Value = $"Total de registros: {list.Count}";
    sheet.Cell(row, 1).Style.Font.SetBold(true).Font.SetFontSize(11);
 
    sheet.Columns().AdjustToContents();
    sheet.Column(3).Width = Math.Max(sheet.Column(3).Width, 18); // que quepa nombre del veterinario
 
    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
}
}