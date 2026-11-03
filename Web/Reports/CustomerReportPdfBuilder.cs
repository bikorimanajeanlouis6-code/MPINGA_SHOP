using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class CustomerReportPdfBuilder
{
    public static byte[] CreateQrCode(string content)
        => ReportBranding.CreateQrCode(content);

    public static byte[] CreatePdf(
        IReadOnlyList<GetCustomerDTO> customers,
        string sexFilter,
        string dateRange,
        byte[] qrCode)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(document => document.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(32);
            page.MarginBottom(90);
            page.DefaultTextStyle(style => style.FontSize(9));

            page.Header().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(header =>
                    {
                        header.Item().Text("Customer Report").FontSize(20).SemiBold();
                        header.Item().PaddingTop(6).Text($"Sex: {sexFilter}    Dates: {dateRange}");
                        header.Item().PaddingTop(3).Text($"Customers: {customers.Count}    Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(12).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                content.Item().PaddingTop(12).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(48);
                        columns.RelativeColumn(1.5f);
                        columns.ConstantColumn(58);
                        columns.RelativeColumn(1.2f);
                        columns.RelativeColumn(1.7f);
                        columns.ConstantColumn(86);
                        columns.RelativeColumn(1.1f);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Reference");
                        HeaderCell(header.Cell(), "Name");
                        HeaderCell(header.Cell(), "Sex");
                        HeaderCell(header.Cell(), "Phone");
                        HeaderCell(header.Cell(), "Email");
                        HeaderCell(header.Cell(), "Registered");
                        HeaderCell(header.Cell(), "Balance");
                    });

                    foreach (var customer in customers)
                    {
                        BodyCell(table.Cell(), EntityReferenceFormatter.Format(EntityReferenceType.Customer, customer.Id));
                        BodyCell(table.Cell(), customer.CustomerName);
                        BodyCell(table.Cell(), customer.Sex);
                        BodyCell(table.Cell(), customer.PhoneNumber);
                        BodyCell(table.Cell(), customer.Email);
                        BodyCell(table.Cell(), customer.DateAdded.ToString("dd MMM yyyy"));
                        BodyCell(table.Cell(), FormatBalance(customer.Balance));
                    }
                });

            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static void HeaderCell(IContainer container, string text) =>
        container.Background(Colors.Grey.Lighten3)
            .Padding(5)
            .Text(text)
            .SemiBold();

    private static void BodyCell(IContainer container, string? text) =>
        container.BorderBottom(0.5f)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5)
            .Text(text ?? string.Empty);

    private static string FormatBalance(string? balance)
    {
        var amount = decimal.TryParse(balance, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var value)
            ? value
            : decimal.TryParse(balance, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out value) ? value : 0m;

        return MoneyFormatter.Format(amount);
    }
}