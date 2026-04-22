// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Services;

// Defines a contract that other classes must follow.
public interface IFileValidationService
// Marks the beginning or end of a block in C#.
{
    void ValidatePdf(IFormFile? file);
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class FileValidationService : IFileValidationService
// Marks the beginning or end of a block in C#.
{
    // Declares a method or property that other parts of the app can use.
    public void ValidatePdf(IFormFile? file)
    // Marks the beginning or end of a block in C#.
    {
        // Checks a rule before continuing, so invalid states are stopped early.
        if (file is null || file.Length == 0)
        // Marks the beginning or end of a block in C#.
        {
            throw new InvalidOperationException("Please choose a PDF file.");
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var extension = Path.GetExtension(file.FileName);
        // Checks a rule before continuing, so invalid states are stopped early.
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        // Marks the beginning or end of a block in C#.
        {
            throw new InvalidOperationException("Only .pdf files are allowed.");
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var contentType = file.ContentType?.Trim().ToLowerInvariant();
        // Checks a rule before continuing, so invalid states are stopped early.
        if (!string.IsNullOrWhiteSpace(contentType) &&
            contentType is not "application/pdf" and not "application/x-pdf")
        // Marks the beginning or end of a block in C#.
        {
            throw new InvalidOperationException("Only PDF content is allowed.");
        // Marks the beginning or end of a block in C#.
        }
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
