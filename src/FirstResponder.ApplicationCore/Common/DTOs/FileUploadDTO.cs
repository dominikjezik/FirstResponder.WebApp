namespace FirstResponder.ApplicationCore.Common.DTOs;

public class FileUploadDto
{
    public required Stream FileStream { get; set; }
    
    public required string Extension { get; set; }
}