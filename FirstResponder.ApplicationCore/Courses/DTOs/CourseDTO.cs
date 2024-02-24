namespace FirstResponder.ApplicationCore.Courses.DTOs;

public class CourseDTO
{
    public Guid CourseId { get; set; }
    
    public CourseFormDTO CourseForm { get; set; }
    
    // TODO: participants
}