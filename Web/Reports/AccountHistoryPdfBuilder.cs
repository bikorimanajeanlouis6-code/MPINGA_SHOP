using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public static class AccountHistoryPdfBuilder
{
    public static byte[] CreatePdf(
        string accountName,
        string accountReference,
        string periodDescription,
        decimal currentBalance,
        IReadOnlyList<AccountHistoryTransaction> transactions)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var moneyIn = transactions.Where(transaction => transaction.IsIncoming).Sum(transaction => transaction.Amount);
        var moneyOut = transactions.Where(transaction => !transaction.IsIncoming).Sum(transaction => transaction.Amount);
        var qrCode = ReportBranding.CreateQrCode(
            $"Account Transaction History\nAccount: {accountName}\nReference: {accountReference}\nPeriod: {periodDescription}\nCurrent balance: {FormatMoney(currentBalance)}\nMoney in: {FormatMoney(moneyIn)}\nMoney out: {FormatMoney(moneyOut)}");

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
                        header.Item().Text("Account Transaction History").FontSize(20).SemiBold();
                        header.Item().PaddingTop(4).Text($"{accountName} · {accountReference}");
                        header.Item().PaddingTop(3).Text($"Period: {periodDescription}");
                        header.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                content.Item().PaddingTop(12).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    SummaryCell(table.Cell(), "Current balance", FormatMoney(currentBalance), Colors.Blue.Lighten4);
                    SummaryCell(table.Cell(), "Money in", FormatMoney(moneyIn), Colors.Green.Lighten4);
                    SummaryCell(table.Cell(), "Money out", FormatMoney(moneyOut), Colors.Orange.Lighten4);
                    SummaryCell(table.Cell(), "Net movement", FormatMoney(moneyIn - moneyOut), Colors.Grey.Lighten3);
                });

                content.Item().PaddingTop(16).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(82);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1.3f);
                        columns.ConstantColumn(90);
                        columns.ConstantColumn(86);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Reference");
                        HeaderCell(header.Cell(), "Description");
                        HeaderCell(header.Cell(), "Type");
                        HeaderCell(header.Cell(), "Date");
                        HeaderCell(header.Cell(), "Amount");
                        HeaderCell(header.Cell(), "Recorded By");
                    });

                    foreach (var transaction in transactions)
                    {
                        BodyCell(table.Cell(), transaction.Reference);
                        BodyCell(table.Cell(), transaction.Description);
                        BodyCell(table.Cell(), transaction.Type);
                        BodyCell(table.Cell(), transaction.Date.ToString("dd MMM yyyy HH:mm"));
                        BodyCell(table.Cell(), $"{(transaction.IsIncoming ? "+" : "-")}{FormatMoney(transaction.Amount)}");
                        BodyCell(table.Cell(), transaction.User);
                    }

                    if (transactions.Count == 0)
                        BodyCell(table.Cell().ColumnSpan(6), "No transactions were recorded during this period.");
                });
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static string FormatMoney(decimal amount) => $"{MoneyFormatter.Format(amount)} RWF";

    private static void SummaryCell(IContainer container, string label, string value, string background) =>
        container.Background(background)
            .Padding(9)
            .Column(column =>
            {
                column.Item().Text(label).FontColor(Colors.Grey.Darken2);
                column.Item().PaddingTop(3).Text(value).FontSize(13).SemiBold();
            });

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
