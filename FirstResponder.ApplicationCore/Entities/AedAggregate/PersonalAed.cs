using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class PersonalAed : Aed
{
    [Required(ErrorMessage = "Vlastník je povinný údaj.")]
    public Guid? OwnerId { get; set; }
    public User? Owner { get; set; }


    public override string GetDisplayHolder() => this.Owner?.FullName;
}