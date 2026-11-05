using System.Globalization;
using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class StockDetailsPdfBuilder
{
    public static byte[] CreatePdf(
        GetStockDTO stock,
        IReadOnlyList<StockMovementReportItem> movements)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var stockReference = EntityReferenceFormatter.Format(EntityReferenceType.Stock, stock.Id);
        var stockValue = stock.QuantityAvailable * stock.LastPrice;
        var qrCode = ReportBranding.CreateQrCode(
            $"Stock Details\nProduct: {stock.StockName}\nReference: {stockReference}\nAvailable: {FormatQuantity(stock.QuantityAvailable, stock.Unit)}\nMovements: {movements.Count}");

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
                        header.Item().Text("Stock Details").FontSize(20).SemiBold();
                        header.Item().PaddingTop(5).Text($"{stock.StockName} · {stockReference}");
                        header.Item().PaddingTop(3).Text(
                            $"Category: {(string.IsNullOrWhiteSpace(stock.ProductCategoryName) ? "Uncategorized" : stock.ProductCategoryName)}    Unit: {QuantityUnitFormatter.Format(stock.Unit)}");
                        header.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                content.Item().PaddingTop(12).Row(row =>
                {
                    SummaryItem(row.RelativeItem(), "Received", FormatQuantity(stock.QuantityInStock, stock.Unit));
                    SummaryItem(row.RelativeItem(), "Sold", FormatQuantity(stock.QuantitySold, stock.Unit));
                    SummaryItem(row.RelativeItem(), "Available", FormatQuantity(stock.QuantityAvailable, stock.Unit));
                    SummaryItem(row.RelativeItem(), "Available stock value", $"{MoneyFormatter.Format(stockValue)} RWF");
                });

                content.Item().PaddingTop(18).Text("Stock Movement History").FontSize(14).SemiBold();
                content.Item().PaddingTop(6).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(92);
                        columns.RelativeColumn(0.9f);
                        columns.ConstantColumn(72);
                        columns.RelativeColumn(1.3f);
                        columns.RelativeColumn(1.1f);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Date");
                        HeaderCell(header.Cell(), "Movement");
                        HeaderCell(header.Cell(), "Reference");
                        HeaderCell(header.Cell(), "Supplier / Customer");
                        HeaderCell(header.Cell(), "Recorded by");
                        HeaderCell(header.Cell(), "Quantity");
                        HeaderCell(header.Cell(), "Unit price");
                    });

                    foreach (var movement in movements.OrderByDescending(item => item.Date))
                    {
                        BodyCell(table.Cell(), movement.Date.ToString("dd MMM yyyy HH:mm", CultureInfo.CurrentCulture));
                        BodyCell(table.Cell(), movement.IsStockIn ? "Stock in" : "Sale");
                        BodyCell(table.Cell(), movement.Reference);
                        BodyCell(table.Cell(), movement.Party);
                        BodyCell(table.Cell(), movement.UserAdded);
                        BodyCell(table.Cell(), $"{(movement.IsStockIn ? "+" : "-")}{FormatQuantity(movement.Quantity, stock.Unit)}");
                        BodyCell(table.Cell(), $"{MoneyFormatter.Format(movement.UnitPrice)} RWF");
                    }

                    if (movements.Count == 0)
                        BodyCell(table.Cell().ColumnSpan(7), "No stock movements have been recorded.");
                });
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static void SummaryItem(IContainer container, string label, string value) =>
        container.Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(8)
            .Column(column =>
            {
                column.Item().Text(label).FontColor(Colors.Grey.Darken1).FontSize(8);
                column.Item().PaddingTop(3).Text(value).SemiBold();
            });

    private static string FormatQuantity(int quantity, string? unit) =>
        $"{quantity.ToString("N0", CultureInfo.CurrentCulture)} {QuantityUnitFormatter.Format(unit)}";

    private static void HeaderCell(IContainer container, string text) =>
        container.Background(Colors.Grey.Lighten3).Padding(5).Text(text).SemiBold();

    private static void BodyCell(IContainer container, string? text) =>
        container.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(text ?? string.Empty);
}

public sealed record StockMovementReportItem(
    DateTime Date,
    bool IsStockIn,
    string Reference,
    string Party,
    string UserAdded,
    int Quantity,
    decimal UnitPrice);
