using FlowOps.Application.DTOs.WorkItems;
using FlowOps.Application.Interfaces;
using FlowOps.Application.Services;
using FlowOps.Domain.Entities;
using FlowOps.Domain.Enums;
using Moq;

namespace FlowOps.Application.Tests.Services
{
    public class WorkItemServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_Should_Return_WorkItem_When_Found()
        {
            // Arrange
            var workItem = new WorkItem(
                "Implement unit tests",
                "Test application service",
                WorkItemPriority.Medium);

            var repositoryMock = new Mock<IWorkItemRepository>();

            repositoryMock
                .Setup(repo => repo.GetByIdAsync(
                    workItem.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(workItem);

            var service = new WorkItemService(repositoryMock.Object);

            // Act
            var result = await service.GetByIdAsync(workItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(workItem.Id, result.Id);
            Assert.Equal("Implement unit tests", result.Title);

            repositoryMock.Verify(
                repo => repo.GetByIdAsync(
                    workItem.Id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_NotFound()
        {
            // Arrange
            var workItemId = Guid.NewGuid();

            var repositoryMock = new Mock<IWorkItemRepository>();

            repositoryMock
                .Setup(repo => repo.GetByIdAsync(
                    workItemId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((WorkItem?)null);

            var service = new WorkItemService(repositoryMock.Object);

            // Act
            var result = await service.GetByIdAsync(workItemId);

            // Assert
            Assert.Null(result);

            repositoryMock.Verify(
                repo => repo.GetByIdAsync(
                    workItemId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_And_Save_WorkItem()
        {
            // Arrange
            var repositoryMock = new Mock<IWorkItemRepository>();

            repositoryMock
                .Setup(repo => repo.AddAsync(
                    It.IsAny<WorkItem>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            repositoryMock
                .Setup(repo => repo.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var service = new WorkItemService(repositoryMock.Object);

            var request = new CreateWorkItemRequest
            {
                Title = "Implement CreateAsync unit test",
                Description = "Verify repository interactions",
                Priority = WorkItemPriority.Medium
            };

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.Priority, result.Priority);

            repositoryMock.Verify(
                repo => repo.AddAsync(
                    It.Is<WorkItem>(item =>
                        item.Title == request.Title &&
                        item.Description == request.Description &&
                        item.Priority == request.Priority),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repo => repo.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_WorkItem_When_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IWorkItemRepository>();

            var workItem = new WorkItem(
                "Original title",
                "Original description",
                WorkItemPriority.Low);

            repositoryMock
                .Setup(repo => repo.GetByIdAsync(
                    workItem.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(workItem);

            repositoryMock
                .Setup(repo => repo.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var service = new WorkItemService(repositoryMock.Object);

            // Act
            var result = await service.UpdateAsync(
                workItem.Id,
                "Updated title",
                "Updated description",
                WorkItemPriority.High);

            // Assert
            Assert.True(result);
            Assert.Equal("Updated title", workItem.Title);
            Assert.Equal("Updated description", workItem.Description);
            Assert.Equal(WorkItemPriority.High, workItem.Priority);
            Assert.NotNull(workItem.UpdatedAt);

            repositoryMock.Verify(
                repo => repo.Update(workItem),
                Times.Once);

            repositoryMock.Verify(
                repo => repo.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_Not_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IWorkItemRepository>();

            var workItemId = Guid.NewGuid();

            repositoryMock
                .Setup(repo => repo.GetByIdAsync(
                    workItemId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((WorkItem?)null);

            var service = new WorkItemService(repositoryMock.Object);

            // Act
            var result = await service.UpdateAsync(
                workItemId,
                "Updated title",
                "Updated description",
                WorkItemPriority.High);

            // Assert
            Assert.False(result);

            repositoryMock.Verify(
                repo => repo.Update(It.IsAny<WorkItem>()),
                Times.Never);

            repositoryMock.Verify(
                repo => repo.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
