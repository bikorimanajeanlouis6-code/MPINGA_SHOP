using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class StockReportPdfBuilder
{
    public static byte[] CreatePdf(
        IReadOnlyList<StockReportCategory> categories,
        string periodDescription,
        string categoryDescription)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var qrCode = ReportBranding.CreateQrCode(
            $"Stock Report\nPeriod: {periodDescription}\nCategory: {categoryDescription}\nStock items: {categories.Sum(category => category.Items.Count)}\nCategories: {categories.Count}");

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
                        header.Item().Text("Stock Report by Category").FontSize(20).SemiBold();
                        header.Item().PaddingTop(6).Text($"Period: {periodDescription}");
                        header.Item().PaddingTop(3).Text($"Category: {categoryDescription}");
                        header.Item().PaddingTop(3).Text(
                            $"Stock items: {categories.Sum(category => category.Items.Count)}    Categories: {categories.Count}    Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(12).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                foreach (var category in categories)
                {
                    content.Item().PaddingTop(12).Text(category.CategoryName).FontSize(13).SemiBold();
                    content.Item().PaddingTop(4).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.ConstantColumn(82);
                        });

                        table.Header(header =>
                        {
                            HeaderCell(header.Cell(), "Stock Item");
                            HeaderCell(header.Cell(), "Quantity In");
                            HeaderCell(header.Cell(), "Quantity Sold");
                            HeaderCell(header.Cell(), "Available");
                            HeaderCell(header.Cell(), "Stock Value");
                            HeaderCell(header.Cell(), "Date Added");
                        });

                        foreach (var stock in category.Items)
                        {
                            BodyCell(table.Cell(), stock.StockName);
                            BodyCell(table.Cell(), FormatQuantity(stock.QuantityInStock, stock.Unit));
                            BodyCell(table.Cell(), FormatQuantity(stock.QuantitySold, stock.Unit));
                            BodyCell(table.Cell(), FormatQuantity(stock.QuantityAvailable, stock.Unit));
                            BodyCell(table.Cell(), FormatMoney(stock.QuantityAvailable * stock.LastPrice));
                            BodyCell(table.Cell(), stock.DateAdded.ToString("dd MMM yyyy"));
                        }

                        table.Footer(footer =>
                        {
                            HeaderCell(footer.Cell(), "Category total");
                            BodyCell(footer.Cell(), FormatQuantityByUnit(category.Items, stock => stock.QuantityInStock));
                            BodyCell(footer.Cell(), FormatQuantityByUnit(category.Items, stock => stock.QuantitySold));
                            BodyCell(footer.Cell(), FormatQuantityByUnit(category.Items, stock => stock.QuantityAvailable));
                            BodyCell(footer.Cell(), FormatMoney(category.Items.Sum(stock => stock.QuantityAvailable * stock.LastPrice)));
                            BodyCell(footer.Cell(), string.Empty);
                        });
                    });
                }
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static string FormatMoney(decimal amount) =>
        MoneyFormatter.Format(amount);

    private static string FormatQuantity(int quantity, string? unit) =>
        $"{quantity.ToString("N0", System.Globalization.CultureInfo.CurrentCulture)} {QuantityUnitFormatter.Format(unit)}";

    private static string FormatQuantityByUnit(
        IEnumerable<GetStockDTO> items,
        Func<GetStockDTO, int> quantitySelector)
    {
        var totals = items
            .GroupBy(stock => QuantityUnitFormatter.Format(stock.Unit), StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .Select(group => $"{group.Sum(quantitySelector).ToString("N0", System.Globalization.CultureInfo.CurrentCulture)} {group.Key}");

        return string.Join(", ", totals);
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
}

public sealed record StockReportCategory(string CategoryName, IReadOnlyList<GetStockDTO> Items);
