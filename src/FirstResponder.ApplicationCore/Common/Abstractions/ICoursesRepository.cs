using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface ICoursesRepository
{
    Task<IEnumerable<Course>> GetCourses(int pageNumber, int pageSize, CourseFiltersDTO? filtersDTO = null);
    
    Task<Course?> GetCourseById(Guid courseId);
    
    Task<CourseDTO?> GetCourseDetailsById(Guid courseId);
    
    Task AddCourse(Course course);
    
    Task UpdateCourse(Course course);
    
    Task DeleteCourse(Course course);
    
    Task<IEnumerable<UserWithAssociationInfoDTO>> GetUsersWithCourseInfoAsync(Guid courseId, string searchQuery, int limitResultsCount, bool includeNotInCourse = false);
	
    Task ChangeUsersInCourse(Guid courseId, IEnumerable<Guid> addUsers, IEnumerable<Guid> removeUsers);
}