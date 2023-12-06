using FirstResponder.ApplicationCore.Shared;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IFileService
{
    Task<string> StoreFile(FileUploadDTO fileUploadDTO);

    Task DeleteFile(string fileName);
}