using AutoFixture;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.Identity;
using FirstResponder.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FirstResponder.InfrastructureTests.Repositories;

public class UsersRepositorySearchTests : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> dbContextOptions = 
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "FirstResponderTestDatabase"+ nameof(UsersRepositorySearchTests))
            .Options;
    
    private readonly IFixture _fixture = new Fixture();
    
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    
    public async Task SeedDatabase(ApplicationDbContext context)
    {
        var data = new List<string[]>
        {
            new[] {"john@example.com", "John Doe"},
            new[] {"Henri_Gleichner61@hotmail.com", "Ellis Heathcote"},
            new[] {"Brannon_Doe@hotmail.com", "Carrie Waelchi"},
            new[] {"Richard.Jenkins@yahoo.com", "Earl Thompson"},
            new[] {"hello@yahoo.com", "Jane Doe"},
        };

        foreach (var item in data)
        {
            var user = _fixture.Build<User>()
                .With(u => u.Email, item[0])
                .With(u => u.FullName, item[1])
                .Create();
        
            context.Users.Add(user.ToApplicationUser());
        }
        
        await context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task SearchQueryIsInFullNameOrEmail_ReturnsCorrectUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var result = await usersRepository.GetUsersBySearch("Doe", 100);
        
        // Assert
        result.Should().HaveCount(3);
        
        result.Should().Contain(u => u.FullName == "John Doe");
        result.Should().Contain(u => u.FullName == "Jane Doe");
        result.Should().Contain(u => u.Email == "Brannon_Doe@hotmail.com");
    }
    
    [Fact]
    public async Task SearchQueryWithLimit_ReturnsCorrectCountOfUsers()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var result = await usersRepository.GetUsersBySearch("Doe", 2);
        
        // Assert
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task SearchQueryWithNoMatchingUsers_ReturnsEmptyList()
    {
        // Arrange
        await using var context = new ApplicationDbContext(dbContextOptions);
        await SeedDatabase(context);
        
        var usersRepository = new UsersRepository(context, _userManagerMock.Object);
        
        // Act
        var result = await usersRepository.GetUsersBySearch("DoeNeexistujuci", 100);
        
        // Assert
        result.Should().BeEmpty();
    }


    public void Dispose()
    {
        using var context = new ApplicationDbContext(dbContextOptions);
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}