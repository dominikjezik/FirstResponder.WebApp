using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class PersonalAed : Aed
{
    [Required]
    public Guid? OwnerId { get; set; }
    public User? Owner { get; set; }
}