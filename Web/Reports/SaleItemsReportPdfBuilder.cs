using System.Globalization;
using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class SaleItemsReportPdfBuilder
{
    public static byte[] CreatePdf(IReadOnlyList<GetSaleItemsDTO> operations, string periodDescription)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var total = operations.Sum(operation => ParseAmount(operation.TotalSaleItemPrice));
        var productTotals = operations
            .GroupBy(operation => new
            {
                operation.ProductId,
                operation.SaleItemName,
                operation.SaleItemCategory,
                Unit = DisplayUnit(operation.Unit)
            })
            .OrderByDescending(group => group.Count())
            .ThenByDescending(group => group.Sum(item => ParseQuantity(item.SaleItemQuantity)))
            .ThenBy(group => group.Key.SaleItemName, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var qrCode = ReportBranding.CreateQrCode(
            $"Sale Items Report\nPeriod: {periodDescription}\nOperations: {operations.Count}\nTotal: {MoneyFormatter.Format(total)}");

        return Document.Create(document => document.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(32);
            page.MarginBottom(90);
            page.DefaultTextStyle(style => style.FontSize(8));

            page.Header().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(header =>
                    {
                        header.Item().Text("Sale Items Report").FontSize(20).SemiBold();
                        header.Item().PaddingTop(5).Text($"Period: {periodDescription}");
                        header.Item().PaddingTop(3).Text($"Operations: {operations.Count}    Total: {MoneyFormatter.Format(total)}");
                        header.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}");
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().PaddingTop(10).Column(content =>
            {
                content.Item().Text("Products Sold Summary").FontSize(13).SemiBold();
                content.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1.3f);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Product");
                        HeaderCell(header.Cell(), "Category");
                        HeaderCell(header.Cell(), "Unit");
                        HeaderCell(header.Cell(), "Quantity Sold");
                        HeaderCell(header.Cell(), "Times Sold");
                        HeaderCell(header.Cell(), "Sales Value");
                    });

                    foreach (var group in productTotals)
                    {
                        BodyCell(table.Cell(), group.Key.SaleItemName);
                        BodyCell(table.Cell(), group.Key.SaleItemCategory);
                        BodyCell(table.Cell(), group.Key.Unit);
                        BodyCell(table.Cell(), group.Sum(item => ParseQuantity(item.SaleItemQuantity)).ToString("N0", CultureInfo.CurrentCulture));
                        BodyCell(table.Cell(), group.Count().ToString("N0", CultureInfo.CurrentCulture));
                        BodyCell(table.Cell(), MoneyFormatter.Format(group.Sum(item => ParseAmount(item.TotalSaleItemPrice))));
                    }

                    if (productTotals.Count == 0)
                        BodyCell(table.Cell().ColumnSpan(6), "No products were sold during this period.");
                });

                content.Item().PaddingTop(16).Text("Sale Operation Details").FontSize(13).SemiBold();
                content.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(34);
                        columns.RelativeColumn(1.7f);
                        columns.RelativeColumn(1.3f);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(0.7f);
                        columns.RelativeColumn(1);
                        columns.ConstantColumn(82);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Reference");
                        HeaderCell(header.Cell(), "Product");
                        HeaderCell(header.Cell(), "Category");
                        HeaderCell(header.Cell(), "Customer");
                        HeaderCell(header.Cell(), "Unit Price");
                        HeaderCell(header.Cell(), "Quantity");
                        HeaderCell(header.Cell(), "Total");
                        HeaderCell(header.Cell(), "Recorded");
                        HeaderCell(header.Cell(), "User");
                    });

                    foreach (var item in operations)
                    {
                        BodyCell(table.Cell(), EntityReferenceFormatter.Format(EntityReferenceType.SaleItem, item.Id));
                        BodyCell(table.Cell(), item.SaleItemName);
                        BodyCell(table.Cell(), item.SaleItemCategory);
                        BodyCell(table.Cell(), item.CustomerName);
                        BodyCell(table.Cell(), MoneyFormatter.Format(ParseAmount(item.SaleItemPrice)));
                        BodyCell(table.Cell(), $"{item.SaleItemQuantity} {DisplayUnit(item.Unit)}");
                        BodyCell(table.Cell(), MoneyFormatter.Format(ParseAmount(item.TotalSaleItemPrice)));
                        BodyCell(table.Cell(), item.DateAdded.ToString("dd MMM yyyy HH:mm", CultureInfo.CurrentCulture));
                        BodyCell(table.Cell(), item.UserAdded);
                    }

                    if (operations.Count == 0)
                        BodyCell(table.Cell().ColumnSpan(9), "No sale operations were recorded during this period.");
                });
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static decimal ParseAmount(string? amount) =>
        decimal.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            ? value
            : decimal.TryParse(amount, NumberStyles.Number, CultureInfo.CurrentCulture, out value)
                ? value
                : 0m;

    private static string DisplayUnit(string? unit) =>
        string.IsNullOrWhiteSpace(unit) ? "units" : unit;

    private static int ParseQuantity(string? quantity) =>
        int.TryParse(quantity, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : int.TryParse(quantity, NumberStyles.Integer, CultureInfo.CurrentCulture, out value)
                ? value
                : 0;

    private static void HeaderCell(IContainer container, string text) =>
        container.Background(Colors.Grey.Lighten3).Padding(5).Text(text).SemiBold();

    private static void BodyCell(IContainer container, string? text) =>
        container.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(text ?? string.Empty);
}
