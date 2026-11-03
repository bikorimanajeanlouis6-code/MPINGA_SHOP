using System.Reflection;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Web.Reports;

public static class ReportBranding
{
    private static readonly byte[] LogoBytes = LoadLogo();

    public static byte[] CreateQrCode(string content)
    {
        using var generator = new QRCodeGenerator();
        using var qrData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        return new PngByteQRCode(qrData).GetGraphic(8);
    }

    public static void AddHeaderLogo(IContainer container)
    {
        container.AlignRight()
            .Width(78)
            .Height(48)
            .Image(LogoBytes)
            .FitArea();
    }

    public static void AddFooterBranding(IContainer container, byte[] qrCode)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .AlignCenter()
                .AlignMiddle()
                .Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            row.ConstantItem(72).AlignRight().Column(column =>
            {
                column.Item()
                    .AlignRight()
                    .Width(52)
                    .Height(52)
                    .Image(qrCode)
                    .FitArea();
                column.Item()
                    .AlignRight()
                    .Text("Report QR")
                    .FontSize(7)
                    .FontColor(Colors.Grey.Darken1);
            });
        });
    }

    private static byte[] LoadLogo()
    {
        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Web.Reports.logo.png")
            ?? throw new InvalidOperationException("The embedded report logo could not be found.");
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return buffer.ToArray();
    }
}
