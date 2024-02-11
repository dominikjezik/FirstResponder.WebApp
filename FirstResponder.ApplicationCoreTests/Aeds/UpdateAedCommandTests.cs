using AutoFixture;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Handlers;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Shared;
using FluentAssertions;
using Moq;

namespace FirstResponder.ApplicationCoreTests.Aeds;

public class UpdateAedCommandTests
{
    private readonly IFixture _fixture = new Fixture();
    
    private readonly Mock<IAedRepository> _aedRepositoryMock = new();
    private readonly Mock<IUsersRepository> _usersRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();

    public UpdateAedCommandTests()
    {
        _fixture.Register<FileUploadDto>(() => null);
    }
    
    [Fact]
    public async Task AedNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var aedFormDto = _fixture.Create<AedFormDTO>();
        
        var command = new UpdateAedCommand(aedFormDto);
        var handler = new UpdateAedCommandHandler(_aedRepositoryMock.Object, _usersRepositoryMock.Object, _fileServiceMock.Object);
        
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
        
        _aedRepositoryMock.Verify(foo => foo.UpdateAed(It.IsAny<Aed>()), Times.Never());
    }
}