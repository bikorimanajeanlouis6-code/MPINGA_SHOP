using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class SalesReportPdfBuilder
{
    public static byte[] CreatePdf(
        IReadOnlyList<GetSalesDTO> sales,
        string periodDescription,
        decimal totalConsumed,
        decimal totalPaid,
        decimal totalBalance)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var qrCode = ReportBranding.CreateQrCode(
            $"Sales Report\nPeriod: {periodDescription}\nSales: {sales.Count}\nConsumed: {FormatMoney(totalConsumed)}\nPaid: {FormatMoney(totalPaid)}\nBalance: {FormatMoney(totalBalance)}");

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
                        header.Item().Text("Sales Report").FontSize(20).SemiBold();
                        header.Item().PaddingTop(6).Text($"Period: {periodDescription}");
                        header.Item().PaddingTop(3).Text(
                            $"Sales: {sales.Count}    Total consumed: {FormatMoney(totalConsumed)}    Total paid: {FormatMoney(totalPaid)}    Balance: {FormatMoney(totalBalance)}");
                        header.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(12).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().PaddingTop(12).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(52);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1.1f);
                    columns.RelativeColumn(1.1f);
                    columns.RelativeColumn(1.1f);
                    columns.ConstantColumn(88);
                    columns.RelativeColumn(1.5f);
                });

                table.Header(header =>
                {
                    HeaderCell(header.Cell(), "Reference");
                    HeaderCell(header.Cell(), "Customer");
                    HeaderCell(header.Cell(), "Consumed");
                    HeaderCell(header.Cell(), "Paid");
                    HeaderCell(header.Cell(), "Balance");
                    HeaderCell(header.Cell(), "Sale Date");
                    HeaderCell(header.Cell(), "User Added");
                });

                foreach (var sale in sales)
                {
                    BodyCell(table.Cell(), EntityReferenceFormatter.Format(EntityReferenceType.Sale, sale.Id));
                    BodyCell(table.Cell(), sale.CustomerName);
                    BodyCell(table.Cell(), FormatMoney(ParseAmount(sale.TotalConsumed)));
                    BodyCell(table.Cell(), FormatMoney(ParseAmount(sale.TotalPaid)));
                    BodyCell(table.Cell(), FormatMoney(ParseAmount(sale.Balance)));
                    BodyCell(table.Cell(), sale.SaleDate.ToString("dd MMM yyyy"));
                    BodyCell(table.Cell(), sale.UserAdded);
                }
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    public static decimal ParseAmount(string? amount) =>
        decimal.TryParse(amount, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var value)
            ? value
            : decimal.TryParse(amount, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out value)
                ? value
                : 0m;

    private static string FormatMoney(decimal amount) =>
        MoneyFormatter.Format(amount);

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
}
