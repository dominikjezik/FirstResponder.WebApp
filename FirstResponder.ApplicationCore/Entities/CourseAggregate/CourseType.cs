using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.CourseAggregate;

public class CourseType : BaseEntity<Guid>
{
    [Required]
    public string Name { get; set; }
}