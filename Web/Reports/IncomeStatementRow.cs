namespace Web.Reports;

public sealed record IncomeStatementRow(
    DateTime Date,
    string Reference,
    string Description,
    string Category,
    decimal Amount,
    bool IsIncome);
