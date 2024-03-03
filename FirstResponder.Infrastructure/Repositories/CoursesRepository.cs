using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.DTOs;
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
        return await _dbContext.Courses
            .Where(c => c.Id == courseId)
            .Include(c => c.CourseType)
            .Include(c => c.Participants)
            .Select(c => new
            {
                Course = c,
                Participants = c.Participants
                    .Join(
                        _dbContext.Users,
                        c => c.UserId,
                        u => u.Id,
                        (c, u) => new { c, u }
                    ).OrderBy(c => c.u.FullName)
                    .ToList()
            })
            .Select(result => new CourseDTO
            {
                CourseId = result.Course.Id,
                CourseForm = new CourseFormDTO
                {
                    Name = result.Course.Name,
                    CourseTypeId = result.Course.CourseTypeId,
                    StartDate = result.Course.StartDate,
                    EndDate = result.Course.EndDate,
                    Location = result.Course.Location,
                    Trainer = result.Course.Trainer,
                    Description = result.Course.Description,
                },
                CourseTypeName = result.Course.CourseType.Name,
                Participants = result.Participants
                    .Select(p => new CourseDTO.ParticipantItemDTO
                    {
                        ParticipantId = p.u.Id,
                        FullName = p.u.FullName,
                        Email = p.u.Email,
                        CreatedAt = p.c.CreatedAt
                    }).ToList()
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

    public async Task<IEnumerable<UserWithAssociationInfoDTO>> GetUsersWithCourseInfoAsync(Guid courseId, string searchQuery, int limitResultsCount = 0, bool includeNotInCourse = false)
    {
        var queryable = _dbContext.Users
            .GroupJoin(
                _dbContext.CourseUser.Where(courseUser => courseUser.CourseId == courseId),
                user => user.Id,
                courseUser => courseUser.UserId,
                (user, courseUser) => new { User = user, CourseUser = courseUser }
            )
            .Where(courseUser => 
                courseUser.User.FullName.Contains(searchQuery) || 
                courseUser.User.Email.Contains(searchQuery)
            )
            .Where(courseUser => includeNotInCourse || courseUser.CourseUser.Any())
            .OrderByDescending(courseUser => courseUser.User.CreatedAt)
            .SelectMany(
                courseUser => courseUser.CourseUser.DefaultIfEmpty(),
                (user, courseUser) => new UserWithAssociationInfoDTO
                {
                    UserId = user.User.Id,
                    FullName = user.User.FullName,
                    Email = user.User.Email,
                    IsAssociated = courseUser != null
                }
            );

        if (limitResultsCount > 0)
        {
            queryable = queryable.Take(limitResultsCount);
        }
        
        return await queryable.ToListAsync();
    }

    public async Task ChangeUsersInCourse(Guid courseId, IEnumerable<Guid> addUsers, IEnumerable<Guid> removeUsers)
    {
        // Users that are in the course
        var usersCourse = await _dbContext.CourseUser
            .Where(courseUser => courseUser.CourseId == courseId)
            .Select(courseUser => courseUser.UserId)
            .ToListAsync();
		
        // Filter out already added users
        addUsers = addUsers.Except(usersCourse);
		
        // Filter out users not in the course
        removeUsers = removeUsers.Intersect(usersCourse);
		
        var addCourseUsers = addUsers.Select(userId => new CourseUser
        {
            CourseId = courseId,
            UserId = userId,
            CreatedAt = DateTime.Now
        });
		
        var removeCourseUsers = removeUsers.Select(userId => new CourseUser
        {
            CourseId = courseId,
            UserId = userId
        });
		
        _dbContext.CourseUser.AddRange(addCourseUsers);
        _dbContext.CourseUser.RemoveRange(removeCourseUsers);
		
        await _dbContext.SaveChangesAsync();
    }
}