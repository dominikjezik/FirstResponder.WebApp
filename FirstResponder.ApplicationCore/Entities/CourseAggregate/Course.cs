using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.CourseAggregate;

public class Course : AuditableEntity<Guid>
{
    [Required]
    public string Name { get; set; }
    
    public Guid? CourseTypeId { get; set; }
    public CourseType? CourseType { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public string? Location { get; set; }
    
    public string? Trainer { get; set; }
    
    public string? Description { get; set; }
    
    public List<CourseUser> Participants { get; set; } = new();
}