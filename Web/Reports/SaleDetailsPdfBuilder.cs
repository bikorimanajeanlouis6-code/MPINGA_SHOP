using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using Web.Formatting;

namespace Web.Reports;

public static class SaleDetailsPdfBuilder
{
    public static byte[] CreatePdf(GetSalesDTO sale, IReadOnlyList<GetSaleItemsDTO> items)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var qrCode = ReportBranding.CreateQrCode(
            $"Sale Details\nSale: {EntityReferenceFormatter.Format(EntityReferenceType.Sale, sale.Id)}\nCustomer: {sale.CustomerName}\nSale date: {sale.SaleDate:dd MMM yyyy}\nTotal: {FormatMoney(sale.TotalConsumed)}\nPaid: {FormatMoney(sale.TotalPaid)}\nBalance: {FormatMoney(sale.Balance)}");

        return Document.Create(document => document.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(36);
            page.MarginBottom(90);
            page.DefaultTextStyle(style => style.FontSize(9));

            page.Header().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(header =>
                    {
                        header.Item().Text($"Sale {EntityReferenceFormatter.Format(EntityReferenceType.Sale, sale.Id)}").FontSize(22).SemiBold();
                        header.Item().PaddingTop(4).Text("Sale Details");
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(12).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                content.Item().PaddingTop(14).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    DetailRow(table, "Customer", sale.CustomerName);
                    DetailRow(table, "Sale Date", sale.SaleDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture));
                    DetailRow(table, "Status", sale.Status);
                    DetailRow(table, "Added By", sale.UserAdded);
                    DetailRow(table, "Total", FormatMoney(sale.TotalConsumed), true);
                    DetailRow(table, "Paid", FormatMoney(sale.TotalPaid));
                    DetailRow(table, "Balance", FormatMoney(sale.Balance), true);
                });

                content.Item().PaddingTop(20).Text("Products").FontSize(14).SemiBold();
                content.Item().PaddingTop(6).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1.2f);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Product");
                        HeaderCell(header.Cell(), "Category");
                        HeaderCell(header.Cell(), "Unit Price");
                        HeaderCell(header.Cell(), "Quantity");
                        HeaderCell(header.Cell(), "Line Total");
                    });

                    foreach (var item in items)
                    {
                        BodyCell(table.Cell(), item.SaleItemName);
                        BodyCell(table.Cell(), item.SaleItemCategory);
                        BodyCell(table.Cell(), FormatMoney(item.SaleItemPrice));
                        BodyCell(table.Cell(), FormatQuantity(item.SaleItemQuantity, item.Unit));
                        BodyCell(table.Cell(), FormatMoney(item.TotalSaleItemPrice));
                    }

                    if (items.Count == 0)
                        BodyCell(table.Cell().ColumnSpan(5), "No product lines are recorded for this sale.");
                });
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static void DetailRow(TableDescriptor table, string label, string value, bool emphasize = false)
    {
        var labelCell = table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(6);
        var valueCell = table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(6);
        if (emphasize)
        {
            labelCell.Text(label).SemiBold();
            valueCell.Text(value).SemiBold();
        }
        else
        {
            labelCell.Text(label);
            valueCell.Text(value);
        }
    }

    private static string FormatMoney(string amount) =>
        decimal.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            ? MoneyFormatter.Format(value)
            : decimal.TryParse(amount, NumberStyles.Number, CultureInfo.CurrentCulture, out value)
                ? MoneyFormatter.Format(value)
                : MoneyFormatter.Format(amount);

    private static string FormatQuantity(string quantity, string unit) =>
        $"{(int.TryParse(quantity, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value.ToString("N0", CultureInfo.CurrentCulture) : quantity)} {(string.IsNullOrWhiteSpace(unit) ? "units" : unit)}";

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
