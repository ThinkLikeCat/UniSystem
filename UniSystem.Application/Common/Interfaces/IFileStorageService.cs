namespace UniSystem.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(Guid documentId, string fileName, Stream content, CancellationToken ct = default);
    Task<Stream?> GetAsync(string filePath, CancellationToken ct = default);
    Task DeleteAsync(string filePath, CancellationToken ct = default);
}
