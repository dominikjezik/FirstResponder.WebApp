using FirstResponder.ApplicationCore.Enums;
using FirstResponder.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.DbContext;

public static class DbInitializer
{
    public static void SeedUserRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
        {
            Id = Guid.Parse("6362CED8-0559-4270-8C88-249400928F38"),
            Name = UserType.Responder.ToString(),
            NormalizedName = UserType.Responder.ToString().ToUpper()
        });
        
        modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
        {
            Id = Guid.Parse("BD4FA5AB-762B-481B-BC27-5C2E79105B2E"),
            Name = UserType.Employee.ToString(),
            NormalizedName = UserType.Employee.ToString().ToUpper()
        });
        
    }
}