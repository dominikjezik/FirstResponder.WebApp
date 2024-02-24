using FirstResponder.ApplicationCore.Entities.CourseAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface ICourseTypesRepository
{
    Task<IEnumerable<CourseType>> GetAllCourseTypes();

    Task<CourseType?> GetCourseTypeById(Guid courseId);
    
    Task<bool> CourseTypeExists(string name);
    
    Task AddCourseType(CourseType courseType);
    
    Task UpdateCourseType(CourseType courseType);
    
    Task DeleteCourseType(CourseType courseType);
}