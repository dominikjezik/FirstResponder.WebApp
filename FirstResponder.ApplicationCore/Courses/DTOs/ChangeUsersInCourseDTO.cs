namespace FirstResponder.ApplicationCore.Courses.DTOs;

public class ChangeUsersInCourseDTO
{
    public Guid CourseId { get; set; }
    
    public IEnumerable<Guid> CheckedOnUserIds { get; set; } = new List<Guid>();
    
    public IEnumerable<Guid> CheckedOffUserIds { get; set; } = new List<Guid>();
}