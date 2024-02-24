using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Entities.CourseAggregate;

public class CourseUser
{
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}