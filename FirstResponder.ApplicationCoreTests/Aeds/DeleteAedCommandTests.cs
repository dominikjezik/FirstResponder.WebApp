using AutoFixture;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Handlers;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FluentAssertions;
using Moq;

namespace FirstResponder.ApplicationCoreTests.Aeds;

public class DeleteAedCommandTests
{
    private readonly IFixture _fixture = new Fixture();
    
    private readonly Mock<IAedRepository> _aedRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();

    public DeleteAedCommandTests()
    {
        _fixture.Register<FileUploadDto>(() => null);
    }
    
    [Fact]
    public async Task AedNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var aedId = Guid.NewGuid();
        
        var command = new DeleteAedCommand(aedId);
        var handler = new DeleteAedCommandHandler(_aedRepositoryMock.Object, _fileServiceMock.Object);
        
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
        
        _aedRepositoryMock.Verify(r => r.DeleteAed(It.IsAny<Aed>()), Times.Never());
    }
    
    [Fact]
    public async Task AedFound_DeletesAed()
    {
        // Arrange
        var aedId = Guid.NewGuid();
        
        var command = new DeleteAedCommand(aedId);
        var handler = new DeleteAedCommandHandler(_aedRepositoryMock.Object, _fileServiceMock.Object);
        
        var aed = _fixture.Create<PublicAed>();
        
        _aedRepositoryMock
            .Setup(r => r.GetAedById(It.IsAny<Guid>()))
            .ReturnsAsync(aed);
        
        _aedRepositoryMock
            .Setup(r => r.GetAedPhotos(It.IsAny<Guid>()))
            .ReturnsAsync(new List<AedPhoto>());
        
        //Act
        await handler.Handle(command, CancellationToken.None);
        
        // Assert
        _aedRepositoryMock.Verify(r => r.DeleteAed(aed), Times.Once());
    }
}