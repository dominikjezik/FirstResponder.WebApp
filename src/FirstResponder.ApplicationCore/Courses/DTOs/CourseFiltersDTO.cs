namespace FirstResponder.ApplicationCore.Courses.DTOs;

public class CourseFiltersDTO
{
    public DateTime? From { get; set; }
    
    public DateTime? To { get; set; }
    
    public Guid? TypeId { get; set; }
    
    public string? Name { get; set; }
}