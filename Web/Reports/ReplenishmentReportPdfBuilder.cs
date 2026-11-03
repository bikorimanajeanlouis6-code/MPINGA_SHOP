using System.Globalization;
using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class ReplenishmentReportPdfBuilder
{
    public static byte[] CreatePdf(
        IReadOnlyList<GetReplenishmentDTO> replenishments,
        string dateRange,
        IReadOnlyList<GetPaymentDTO>? payments = null)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var paymentTotals = (payments ?? Array.Empty<GetPaymentDTO>())
            .Where(payment => payment.ReplenishmentId.HasValue)
            .GroupBy(payment => payment.ReplenishmentId!.Value)
            .ToDictionary(group => group.Key, group => group.Sum(payment => payment.PaymentAmount));
        var totalAmount = replenishments.Sum(GetAmount);
        var totalPaid = replenishments.Sum(item =>
            paymentTotals.TryGetValue(item.Id, out var paid) ? paid : 0m);
        var outstanding = Math.Max(0m, totalAmount - totalPaid);
        var qrCode = ReportBranding.CreateQrCode(
            $"Replenishment Report\nPeriod: {dateRange}\nRecords: {replenishments.Count}\nTotal: {FormatMoney(totalAmount)}\nPaid: {FormatMoney(totalPaid)}\nOutstanding: {FormatMoney(outstanding)}");

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
                        header.Item().Text("Replenishment Report").FontSize(20).SemiBold();
                        header.Item().PaddingTop(5).Text($"Period: {dateRange}");
                        header.Item().PaddingTop(3).Text(
                            $"Records: {replenishments.Count}    Total: {FormatMoney(totalAmount)}    Paid: {FormatMoney(totalPaid)}    Outstanding: {FormatMoney(outstanding)}");
                        header.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().PaddingTop(12).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(42);
                    columns.RelativeColumn(1.6f);
                    columns.RelativeColumn(1.4f);
                    columns.RelativeColumn(1.2f);
                    columns.ConstantColumn(75);
                    columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(1.1f);
                    columns.RelativeColumn(1.1f);
                    columns.RelativeColumn(1.1f);
                    columns.RelativeColumn(1.1f);
                });

                table.Header(header =>
                {
                    HeaderCell(header.Cell(), "Reference");
                    HeaderCell(header.Cell(), "Replenishment");
                    HeaderCell(header.Cell(), "Supplier");
                    HeaderCell(header.Cell(), "Phone");
                    HeaderCell(header.Cell(), "Date");
                    HeaderCell(header.Cell(), "Quantity");
                    HeaderCell(header.Cell(), "Unit Price");
                    HeaderCell(header.Cell(), "Total");
                    HeaderCell(header.Cell(), "Paid");
                    HeaderCell(header.Cell(), "Outstanding");
                });

                foreach (var item in replenishments)
                {
                    var paid = paymentTotals.TryGetValue(item.Id, out var amountPaid) ? amountPaid : 0m;
                    var amount = GetAmount(item);
                    BodyCell(table.Cell(), EntityReferenceFormatter.Format(EntityReferenceType.Replenishment, item.Id));
                    BodyCell(table.Cell(), item.ReplenishmentName);
                    BodyCell(table.Cell(), item.SupplierName);
                    BodyCell(table.Cell(), item.SupplierPhone);
                    BodyCell(table.Cell(), item.ReplenishmentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture));
                    BodyCell(table.Cell(), $"{item.QuantityReplenished:N0} {QuantityUnitFormatter.Format(item.Unit)}");
                    BodyCell(table.Cell(), FormatMoney(ParseAmount(item.PurchasePrice)));
                    BodyCell(table.Cell(), FormatMoney(amount));
                    BodyCell(table.Cell(), FormatMoney(paid));
                    BodyCell(table.Cell(), FormatMoney(Math.Max(0m, amount - paid)));
                }

                if (replenishments.Count == 0)
                    BodyCell(table.Cell().ColumnSpan(10), "No replenishments were recorded during this period.");
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static decimal GetAmount(GetReplenishmentDTO item) =>
        decimal.TryParse(item.ReplenishmentAmount, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount)
            ? amount
            : decimal.TryParse(item.TotalPurchasePrice, NumberStyles.Number, CultureInfo.InvariantCulture, out amount)
                ? amount
                : 0m;

    private static decimal ParseAmount(string? value) =>
        decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount)
            ? amount
            : decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out amount)
                ? amount
                : 0m;

    private static string FormatMoney(decimal amount) => $"{MoneyFormatter.Format(amount)} RWF";

    private static void HeaderCell(IContainer container, string text) =>
        container.Background(Colors.Grey.Lighten3).Padding(5).Text(text).SemiBold();

    private static void BodyCell(IContainer container, string? text) =>
        container.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(text ?? string.Empty);
}
