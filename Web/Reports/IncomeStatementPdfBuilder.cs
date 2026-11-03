using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Web.Formatting;

namespace Web.Reports;

public sealed record IncomeStatementExpenseCategory(string CategoryName, int Count, decimal Amount);

public static class IncomeStatementPdfBuilder
{
    public static byte[] CreatePdf(
        IReadOnlyList<IncomeStatementRow> rows,
        IReadOnlyList<IncomeStatementExpenseCategory> expenseCategories,
        string periodDescription,
        string selectionLabel,
        bool includeIncome,
        bool includeExpenses,
        decimal incomeTotal,
        decimal expenseTotal,
        decimal netResult)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var qrCodeContent = $"Income Statement\nShowing: {selectionLabel}\nPeriod: {periodDescription}";
        if (includeIncome)
            qrCodeContent += $"\nIncome: {FormatMoney(incomeTotal)}";
        if (includeExpenses)
            qrCodeContent += $"\nExpenses: {FormatMoney(expenseTotal)}";
        if (includeIncome && includeExpenses)
            qrCodeContent += $"\nNet result: {FormatMoney(netResult)}";
        var qrCode = ReportBranding.CreateQrCode(qrCodeContent);

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
                        header.Item().Text($"Income Statement - {selectionLabel}").FontSize(20).SemiBold();
                        header.Item().PaddingTop(5).Text($"Reporting period: {periodDescription}");
                        header.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontColor(Colors.Grey.Darken1);
                    });
                    ReportBranding.AddHeaderLogo(row.ConstantItem(88));
                });
                column.Item().PaddingTop(12).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                content.Item().PaddingTop(14).Table(table =>
                {
                    var summaryCount = (includeIncome ? 1 : 0)
                        + (includeExpenses ? 1 : 0)
                        + (includeIncome && includeExpenses ? 1 : 0);
                    table.ColumnsDefinition(columns =>
                    {
                        for (var index = 0; index < summaryCount; index++)
                            columns.RelativeColumn();
                    });

                    if (includeIncome)
                        SummaryCell(table.Cell(), "Total income", FormatMoney(incomeTotal), Colors.Green.Lighten4);
                    if (includeExpenses)
                        SummaryCell(table.Cell(), "Total expenses", FormatMoney(expenseTotal), Colors.Orange.Lighten4);
                    if (includeIncome && includeExpenses)
                    {
                        SummaryCell(
                            table.Cell(),
                            netResult >= 0 ? "Net surplus" : "Net deficit",
                            FormatMoney(netResult),
                            netResult >= 0 ? Colors.Green.Lighten4 : Colors.Red.Lighten4);
                    }
                });

                if (includeExpenses)
                {
                    content.Item().PaddingTop(18).Text("Expenses by Category").FontSize(13).SemiBold();
                    content.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            HeaderCell(header.Cell(), "Category");
                            HeaderCell(header.Cell(), "Transactions");
                            HeaderCell(header.Cell(), "Total");
                        });

                        foreach (var category in expenseCategories)
                        {
                            BodyCell(table.Cell(), category.CategoryName);
                            BodyCell(table.Cell(), category.Count.ToString());
                            BodyCell(table.Cell(), FormatMoney(category.Amount));
                        }

                        if (expenseCategories.Count == 0)
                            BodyCell(table.Cell().ColumnSpan(3), "No expenses recorded during this period.");
                    });
                }

                content.Item().PaddingTop(18).Text("Statement Transactions").FontSize(13).SemiBold();
                content.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(100);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1.5f);
                        columns.ConstantColumn(68);
                        columns.ConstantColumn(86);
                        columns.ConstantColumn(94);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Reference");
                        HeaderCell(header.Cell(), "Description");
                        HeaderCell(header.Cell(), "Category");
                        HeaderCell(header.Cell(), "Type");
                        HeaderCell(header.Cell(), "Date");
                        HeaderCell(header.Cell(), "Amount");
                    });

                    foreach (var row in rows)
                    {
                        BodyCell(table.Cell(), row.Reference);
                        BodyCell(table.Cell(), row.Description);
                        BodyCell(table.Cell(), row.Category);
                        BodyCell(table.Cell(), row.IsIncome ? "Income" : "Expense");
                        BodyCell(table.Cell(), row.Date.ToString("dd MMM yyyy"));
                        BodyCell(table.Cell(), FormatMoney(row.Amount));
                    }

                    if (rows.Count == 0)
                        BodyCell(table.Cell().ColumnSpan(6), "No transactions recorded during this period.");
                });
            });

            ReportBranding.AddFooterBranding(page.Footer(), qrCode);
        })).GeneratePdf();
    }

    private static string FormatMoney(decimal amount) => MoneyFormatter.Format(amount);

    private static void SummaryCell(IContainer container, string label, string amount, string background) =>
        container.Background(background)
            .Padding(10)
            .Column(column =>
            {
                column.Item().Text(label).FontColor(Colors.Grey.Darken2);
                column.Item().PaddingTop(4).Text(amount).FontSize(14).SemiBold();
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
