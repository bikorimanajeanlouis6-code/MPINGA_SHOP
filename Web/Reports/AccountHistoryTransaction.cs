namespace Web.Reports;

public sealed record AccountHistoryTransaction(
    string Reference,
    string Description,
    string Type,
    DateTime Date,
    decimal Amount,
    bool IsIncoming,
    string User);
