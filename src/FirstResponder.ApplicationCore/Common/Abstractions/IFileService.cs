using FirstResponder.ApplicationCore.Common.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IFileService
{
    Task<string> StoreFile(FileUploadDto fileUploadDTO);

    Task DeleteFile(string fileName);
}