using AutoFixture;
using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Handlers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Enums;
using FirstResponder.ApplicationCore.Exceptions;
using FluentAssertions;
using Moq;

namespace FirstResponder.ApplicationCoreTests.Aeds;

public class CreateAedCommandTests
{
    private readonly IFixture _fixture = new Fixture();
    
    private readonly Mock<IAedRepository> _aedRepositoryMock = new();
    private readonly Mock<IUsersRepository> _usersRepositoryMock = new();
    
    [Fact]
    public async Task InvalidOwnerId_ThrowsValidationException()
    {
        // Arrange
        var aedFormDto = _fixture.Build<AedFormDTO>()
            .With(a => a.GeneralType, AedGeneralType.Personal)
            .With(a => a.OwnerId, Guid.NewGuid())
            .Create();
        
        var command = new CreateAedCommand(aedFormDto);
        var handler = new CreateAedCommandHandler(_aedRepositoryMock.Object, _usersRepositoryMock.Object);
        
        _usersRepositoryMock
            .Setup(r => r.UserExists(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        
        //Act
        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };
        
        // Assert
        await action.Should().ThrowAsync<EntityValidationException>()
            .Where(e =>
                e.ValidationErrors.ContainsKey("OwnerId") &&
                e.ValidationErrors["OwnerId"] == "Owner not found");
        
        _aedRepositoryMock.Verify(foo => foo.AddAed(It.IsAny<Aed>()), Times.Never());
    }
    
    [Fact]
    public async Task ValidOwnerId_CreatesPersonalAed()
    {
        // Arrange
        var aedFormDto = _fixture.Build<AedFormDTO>()
            .With(a => a.GeneralType, AedGeneralType.Personal)
            .With(a => a.OwnerId, Guid.NewGuid())
            .Create();
        
        var command = new CreateAedCommand(aedFormDto);
        var handler = new CreateAedCommandHandler(_aedRepositoryMock.Object, _usersRepositoryMock.Object);

        var expectedAedId = Guid.NewGuid();
        
        _usersRepositoryMock
            .Setup(r => r.UserExists(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        
        _aedRepositoryMock
            .Setup(r => r.AddAed(It.IsAny<Aed>()))
            .Callback<Aed>(r => r.Id = expectedAedId);
        
        //Act
        var aed = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        _aedRepositoryMock.Verify(foo => foo.AddAed(It.IsAny<Aed>()), Times.Once());
        aed.Id.Should().Be(expectedAedId);
    }
    
    [Fact]
    public async Task MissingOwnerIdDoesNotMatter_CreatesPublicAed()
    {
        // Arrange
        var aedFormDto = _fixture.Build<AedFormDTO>()
            .With(a => a.GeneralType, AedGeneralType.Public)
            .With(a => a.OwnerId, Guid.Empty)
            .With(a => a.ContactPersonEmail, "test@example.com")
            .Create();
        
        var command = new CreateAedCommand(aedFormDto);
        var handler = new CreateAedCommandHandler(_aedRepositoryMock.Object, _usersRepositoryMock.Object);

        var expectedAedId = Guid.NewGuid();
        
        _usersRepositoryMock
            .Setup(r => r.UserExists(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        
        _aedRepositoryMock
            .Setup(r => r.AddAed(It.IsAny<Aed>()))
            .Callback<Aed>(r => r.Id = expectedAedId);
        
        //Act
        var aed = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        _aedRepositoryMock.Verify(foo => foo.AddAed(It.IsAny<Aed>()), Times.Once());
        aed.Id.Should().Be(expectedAedId);
    }
    
}