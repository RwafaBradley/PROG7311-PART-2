using GLMS.Web.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace GLMS.Web.Tests;

public class FileValidationTests
{
    [Fact]
    public void ValidatePdf_AllowsPdfFiles()
    {
        var service = new FileValidationService();

        var bytes = new byte[] { 1, 2, 3 };
        using var stream = new MemoryStream(bytes);

        IFormFile file = new FormFile(stream, 0, bytes.Length, "file", "agreement.pdf")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };

        var exception = Record.Exception(() => service.ValidatePdf(file));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidatePdf_ThrowsForExeFiles()
    {
        var service = new FileValidationService();

        var bytes = new byte[] { 1, 2, 3 };
        using var stream = new MemoryStream(bytes);

        IFormFile file = new FormFile(stream, 0, bytes.Length, "file", "danger.exe")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };

        var exception = Assert.Throws<InvalidOperationException>(() => service.ValidatePdf(file));

        Assert.Contains("Only .pdf files are allowed", exception.Message);
    }
}