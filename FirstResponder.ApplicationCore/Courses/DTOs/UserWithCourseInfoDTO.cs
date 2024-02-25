namespace FirstResponder.ApplicationCore.Courses.DTOs;

public class UserWithCourseInfoDTO
{
    public Guid UserId { get; set; }
	
    public string FullName { get; set; }

    public string Email { get; set; }
	
    public bool IsInCourse { get; set; }
}