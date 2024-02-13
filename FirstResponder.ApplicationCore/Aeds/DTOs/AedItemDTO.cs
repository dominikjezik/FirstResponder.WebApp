namespace FirstResponder.ApplicationCore.Aeds.DTOs;

public class AedItemDTO
{
    public Guid Id { get; set; }
    
    public string State { get; set; }
    
    public string DisplayState { get; set; }
    
    public string Holder { get; set; }
    
    public string CreatedAt { get; set; }
    
    public string SerialNumber { get; set; }
    
    public string Manufacturer { get; set; }
}