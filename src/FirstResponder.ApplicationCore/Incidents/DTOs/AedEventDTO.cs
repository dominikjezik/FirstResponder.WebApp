namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class AedEventDTO
{
    public Guid AedId { get; set; }
    
    public DateTime EventDate { get; set; }
    
    public string HolderName { get; set; }
    
    public string Type { get; set; }
}