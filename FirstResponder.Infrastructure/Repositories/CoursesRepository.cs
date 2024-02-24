using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class CoursesRepository : ICoursesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CoursesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Course>> GetCourses(int pageNumber, int pageSize, CourseFiltersDTO? filtersDTO = null)
    {
        var query = _dbContext.Courses
            .OrderByDescending(c => c.CreatedAt)
            .Include(c => c.CourseType)
            .AsQueryable();

        if (filtersDTO != null)
        {
            query = query
                .Where( c =>
                    (filtersDTO.From == null || c.StartDate >= filtersDTO.From) &&
                    (filtersDTO.To == null || c.EndDate <= filtersDTO.To) &&
                    (filtersDTO.TypeId == null || c.CourseTypeId == filtersDTO.TypeId) &&
                    (string.IsNullOrEmpty(filtersDTO.Name) || c.Name.Contains(filtersDTO.Name))
                );
        }
        
        return await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Course?> GetCourseById(Guid courseId)
    {
        return await _dbContext.Courses
            .Include(c => c.CourseType)
            .FirstOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<CourseDTO?> GetCourseDetailsById(Guid courseId)
    {
        // TODO: Nacitat ucastnikov
        
        return await _dbContext.Courses
            .Where(c => c.Id == courseId)
            .Select(c => new CourseDTO
            {
                CourseId = c.Id,
                CourseForm = new CourseFormDTO
                {
                    Name = c.Name,
                    CourseTypeId = c.CourseTypeId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Location = c.Location,
                    Trainer = c.Trainer,
                    Description = c.Description,
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task AddCourse(Course course)
    {
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourse(Course course)
    {
        _dbContext.Courses.Update(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCourse(Course course)
    {
        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync();
    }
}