namespace FirstResponder.ApplicationCore.Shared;

public class FileUploadDTO
{
    public required Stream FileStream { get; set; }
    
    public required string Extension { get; set; }
}