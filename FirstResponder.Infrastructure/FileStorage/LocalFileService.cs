using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Shared;
using Microsoft.Extensions.Options;

namespace FirstResponder.Infrastructure.FileStorage;

public class LocalFileService : IFileService
{
    private readonly LocalFileServiceOptions options;
    
    public LocalFileService(IOptions<LocalFileServiceOptions> options)
    {
        this.options = options.Value;

        if (string.IsNullOrEmpty(this.options.UploadsFolderPath))
        {
            throw new InvalidOperationException("No uploads folder path has been configured for the LocalFileService.");
        }
    }
    
    public async Task<string> StoreFile(FileUploadDTO fileUploadDTO)
    {
        var fileName = Guid.NewGuid() + fileUploadDTO.Extension;
        var filePathOnDisk = Path.Combine(options.UploadsFolderPath, fileName);

        Directory.CreateDirectory(options.UploadsFolderPath);

        using var fileStream = new FileStream(filePathOnDisk, FileMode.Create);

        await fileUploadDTO.FileStream.CopyToAsync(fileStream);
        
        return fileName;
    }

    public async Task DeleteFile(string fileName)
    {
        var bannerPathOnDisk = Path.Combine(options.UploadsFolderPath, fileName);

        if (File.Exists(bannerPathOnDisk))
        {
            File.Delete(bannerPathOnDisk);
        }
    }
}