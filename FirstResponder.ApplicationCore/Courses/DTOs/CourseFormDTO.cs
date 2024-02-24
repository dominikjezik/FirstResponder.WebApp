using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;

namespace FirstResponder.ApplicationCore.Courses.DTOs;

public class CourseFormDTO
{
    [Required]
    public string Name { get; set; }
    
    public Guid? CourseTypeId { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public string? Location { get; set; }
    
    public string? Trainer { get; set; }
    
    public string? Description { get; set; }
    
    public Course ToCourse(Course? targetCourse = null)
    {
        Course? course = null;

        if (targetCourse == null)
        {
            course = new Course();
        }
        else
        {
            course = targetCourse;
        }

        course.Name = Name;
        course.CourseTypeId = CourseTypeId;
        course.StartDate = StartDate;
        course.EndDate = EndDate;
        course.Location = Location;
        course.Trainer = Trainer;
        course.Description = Description;

        return course;
    }
}