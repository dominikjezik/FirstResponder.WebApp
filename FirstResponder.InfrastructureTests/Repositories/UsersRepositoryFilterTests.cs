using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.Identity;
using FirstResponder.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FirstResponder.InfrastructureTests.Repositories;

public class UsersRepositoryFilterTests : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> dbContextOptions = 
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "FirstResponderTestDatabase" + nameof(UsersRepositoryFilterTests))
            .Options;
    
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

    public async Task SeedDatabase(ApplicationDbContext context)
    {
        var data = new List<User>
        {
            new User { Email = "john@example.com", FullName = "John Doe", PhoneNumber = "0905123456", Type = UserType.Responder },
            new User { Email = "Henri_Gleichner61@hotmail.com", FullName = "Ellis Heathcote", PhoneNumber = "0905134567", Type = UserType.Responder },
            new User { Email = "Brannon_Doe@hotmail.com", FullName = "Carrie Waelchi", PhoneNumber = "0905145678", Type = UserType.Employee },
            new User { Email = "Richard.Jenkins@yahoo.com", FullName = "Earl Thompson", PhoneNumber = "0905234567", Type = UserType.Employee }, 
            new User { Email = "hello@yahoo.com", FullName = "Jane Doe", PhoneNumber = "0905345678", Type = UserType.Default }, 
        };

        foreach (var user in data)
        {
            context.Users.Add(user.ToApplicationUser());
        }
        
        await context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task EmptyFilter_ReturnsAllUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var users = await usersRepository.GetUserItems(0, 100, new UserItemFiltersDTO());
        
        // Assert
        users.Should().HaveCount(5);
    }
    
    [Fact]
    public async Task FilterByFullName_ReturnsFilteredUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var users = await usersRepository.GetUserItems(0, 100, new UserItemFiltersDTO { FullName = "Doe" });
        
        // Assert
        users.Should().HaveCount(2);
        
        users.Should().Contain(u => u.FullName == "John Doe");
        users.Should().Contain(u => u.FullName == "Jane Doe");
    }
    
    [Fact]
    public async Task FilterByUserType_ReturnsFilteredUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var users = await usersRepository.GetUserItems(0, 100, new UserItemFiltersDTO { Type = UserType.Employee });
        
        // Assert
        users.Should().HaveCount(2);
        
        users.Should().Contain(u => u.FullName == "Carrie Waelchi");
        users.Should().Contain(u => u.FullName == "Earl Thompson");
    }
    
    [Fact]
    public async Task CombinedFilter_ReturnsFilteredUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var users = await usersRepository.GetUserItems(0, 100, new UserItemFiltersDTO { FullName = "Doe", Type = UserType.Responder });
        
        // Assert
        users.Should().HaveCount(1);
        
        users.Should().Contain(u => u.FullName == "John Doe");
    }
    
    [Fact]
    public async Task FilterWithNoMatchingUsers_ReturnsEmptyList()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var users = await usersRepository.GetUserItems(0, 100, new UserItemFiltersDTO { FullName = "John", Type = UserType.Employee});
        
        // Assert
        users.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task PaginationWithoutFilters_ReturnsCorrectUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var usersPage0 = await usersRepository.GetUserItems(0, 2, new UserItemFiltersDTO());
        var usersPage1 = await usersRepository.GetUserItems(1, 2, new UserItemFiltersDTO());
        var usersPage2 = await usersRepository.GetUserItems(2, 2, new UserItemFiltersDTO());
        var usersPage3 = await usersRepository.GetUserItems(3, 2, new UserItemFiltersDTO());
        
        // Assert
        usersPage0.Should().HaveCount(2);
        usersPage1.Should().HaveCount(2);
        usersPage2.Should().HaveCount(1);
        usersPage3.Should().HaveCount(0);

        usersPage0.Should().NotContain(usersPage1);
        usersPage0.Should().NotContain(usersPage2);
        
        usersPage1.Should().NotContain(usersPage0);
        usersPage1.Should().NotContain(usersPage2);
        
        usersPage2.Should().NotContain(usersPage0);
        usersPage2.Should().NotContain(usersPage1);
    }
    
    [Fact]
    public async Task PaginationWithFilters_ReturnsCorrectUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);

        var filters = new UserItemFiltersDTO { PhoneNumber = "09051" };
        
        // Act
        var usersPage0 = await usersRepository.GetUserItems(0, 2, filters);
        var usersPage1 = await usersRepository.GetUserItems(1, 2, filters);
        var usersPage2 = await usersRepository.GetUserItems(2, 2, filters);
        
        // Assert
        usersPage0.Should().HaveCount(2);
        usersPage1.Should().HaveCount(1);
        usersPage2.Should().HaveCount(0);
        
        usersPage0.Should().NotContain(usersPage1);
        usersPage1.Should().NotContain(usersPage0);
        
        usersPage0.Should().AllSatisfy(u => u.PhoneNumber.StartsWith("09051").Should().BeTrue());
        usersPage1.Should().AllSatisfy(u => u.PhoneNumber.StartsWith("09051").Should().BeTrue());
    }

    public void Dispose()
    {
        using var context = new ApplicationDbContext(dbContextOptions);
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}