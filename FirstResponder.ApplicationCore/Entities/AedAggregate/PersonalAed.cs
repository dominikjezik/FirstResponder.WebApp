using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class PersonalAed : Aed
{
    [Required]
    public Guid? OwnerId { get; set; }

    // TODO
    //public User Owner { get; set; }
}