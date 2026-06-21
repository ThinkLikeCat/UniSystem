using Microsoft.Extensions.Configuration;
using UniSystem.Application.Common.Interfaces;

namespace UniSystem.Infrastructure;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Guid documentId, string fileName, Stream content, CancellationToken ct = default)
    {
        var dir = Path.Combine(_basePath, documentId.ToString());
        Directory.CreateDirectory(dir);

        var uniqueName = $"{Guid.NewGuid()}_{fileName}";
        var fullPath = Path.Combine(dir, uniqueName);

        await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await content.CopyToAsync(stream, ct);

        return fullPath;
    }

    public Task<Stream?> GetAsync(string filePath, CancellationToken ct = default)
    {
        if (!File.Exists(filePath))
            return Task.FromResult<Stream?>(null);

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return Task.FromResult<Stream?>(stream);
    }

    public Task DeleteAsync(string filePath, CancellationToken ct = default)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }
}
