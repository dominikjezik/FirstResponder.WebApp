using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class PublicAed : Aed
{
    [Required]
    public string? Holder { get; set; }
    
    // TODO: Pridať ostatné vlastnosti
    
    public override string GetDisplayHolder() => Holder;
}