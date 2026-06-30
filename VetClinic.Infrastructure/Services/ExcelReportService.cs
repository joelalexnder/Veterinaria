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

        // Título
        sheet.Cell(1, 1).Value = "REPORTE DE VACUNACIONES PENDIENTES - VetClinic Pro";
        sheet.Range(1, 1, 1, 7).Merge();
        sheet.Cell(1, 1).Style
            .Font.SetBold(true)
            .Font.SetFontSize(14)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Fill.SetBackgroundColor(XLColor.FromHtml("#70AD47"))
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
            "Mascota", "Vacuna", "Veterinario",
            "Lote", "Fecha Aplicación", "Próximo Refuerzo", "Días Vencida"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = sheet.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style
                .Font.SetBold(true)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#70AD47"))
                .Font.SetFontColor(XLColor.White)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        }

        // Datos
        int row = 5;
        foreach (var item in data)
        {
            sheet.Cell(row, 1).Value = item.PetName;
            sheet.Cell(row, 2).Value = item.VaccineName;
            sheet.Cell(row, 3).Value = item.VeterinarianName;
            sheet.Cell(row, 4).Value = item.BatchNumber ?? "N/A";
            sheet.Cell(row, 5).Value = item.ApplicationDate?.ToString("dd/MM/yyyy") ?? "N/A";
            sheet.Cell(row, 6).Value = item.NextBoosterDate?.ToString("dd/MM/yyyy") ?? "N/A";
            sheet.Cell(row, 7).Value = item.DaysOverdue;

            // Rojo si está muy vencida
            var overdueCell = sheet.Cell(row, 7);
            overdueCell.Style.Fill.SetBackgroundColor(
                item.DaysOverdue > 30
                    ? XLColor.FromHtml("#FCE4D6")
                    : XLColor.FromHtml("#FFF2CC"));
            overdueCell.Style.Font.SetBold(item.DaysOverdue > 30);

            sheet.Range(row, 1, row, 7).Style
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Hair);

            row++;
        }

        sheet.Cell(row + 1, 1).Value = $"Total pendientes: {data.Count()}";
        sheet.Cell(row + 1, 1).Style.Font.SetBold(true);

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}