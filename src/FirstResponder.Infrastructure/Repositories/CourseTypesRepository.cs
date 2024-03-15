using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class CourseTypesRepository : ICourseTypesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourseTypesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<CourseType>> GetAllCourseTypes()
    {
        return await _dbContext.CourseTypes
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CourseType?> GetCourseTypeById(Guid courseId)
    {
        return await _dbContext.CourseTypes
            .Where(c => c.Id == courseId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CourseTypeExists(string name)
    {
        return await _dbContext.CourseTypes
            .Where(c => c.Name == name)
            .AnyAsync();
    }

    public async Task AddCourseType(CourseType courseType)
    {
        _dbContext.CourseTypes.Add(courseType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseType(CourseType courseType)
    {
        _dbContext.Update(courseType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCourseType(CourseType courseType)
    {
        _dbContext.CourseTypes.Remove(courseType);
        await _dbContext.SaveChangesAsync();
    }
}