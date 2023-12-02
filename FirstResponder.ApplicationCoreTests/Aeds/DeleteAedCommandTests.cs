using AutoFixture;
using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Handlers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FluentAssertions;
using Moq;

namespace FirstResponder.ApplicationCoreTests.Aeds;

public class DeleteAedCommandTests
{
    private readonly IFixture _fixture = new Fixture();
    
    private readonly Mock<IAedRepository> _aedRepositoryMock = new();
    
    [Fact]
    public async Task AedNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var aedId = Guid.NewGuid();
        
        var command = new DeleteAedCommand(aedId.ToString());
        var handler = new DeleteAedCommandHandler(_aedRepositoryMock.Object);
        
        _aedRepositoryMock
            .Setup(r => r.GetAedById(It.IsAny<Guid>()))
            .ReturnsAsync((Aed)null);
        
        //Act
        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };
        
        // Assert
        await action.Should().ThrowAsync<EntityNotFoundException>();
        
        _aedRepositoryMock.Verify(foo => foo.DeleteAed(It.IsAny<Aed>()), Times.Never());
    }
    
    [Fact]
    public async Task AedFound_DeletesAed()
    {
        // Arrange
        var aedId = Guid.NewGuid();
        
        var command = new DeleteAedCommand(aedId.ToString());
        var handler = new DeleteAedCommandHandler(_aedRepositoryMock.Object);
        
        var aed = _fixture.Create<PublicAed>();
        
        _aedRepositoryMock
            .Setup(r => r.GetAedById(It.IsAny<Guid>()))
            .ReturnsAsync(aed);
        
        //Act
        await handler.Handle(command, CancellationToken.None);
        
        // Assert
        _aedRepositoryMock.Verify(foo => foo.DeleteAed(aed), Times.Once());
    }
    
    [Fact]
    public async Task InvalidAedId_ThrowsArgumentException()
    {
        // Arrange
        var aedId = "totoUrciteNieJeGuid";
        
        var command = new DeleteAedCommand(aedId);
        var handler = new DeleteAedCommandHandler(_aedRepositoryMock.Object);
        
        //Act
        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };
        
        // Assert
        await action.Should().ThrowAsync<ArgumentException>()
            .Where(e => e.Message == "Invalid aedId Guid format.");
        
        _aedRepositoryMock.Verify(foo => foo.DeleteAed(It.IsAny<Aed>()), Times.Never());
    }
    
}