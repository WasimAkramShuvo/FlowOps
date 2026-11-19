using FlowOps.Domain.Entities;
using FlowOps.Domain.Enums;

namespace FlowOps.Domain.Tests
{
    public class WorkItemTests
    {
        [Fact]
        public void Constructor_Should_Create_WorkItem_With_Correct_Values()
        {
            // Arrange
            string title = "Implement unit tests";
            string description = "Test FlowOps domain entities";
            var priority = WorkItemPriority.Medium;

            // Act
            var workItem = new WorkItem(
                title,
                description,
                priority);

            // Assert
            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(title, workItem.Title);
            Assert.Equal(description, workItem.Description);
            Assert.Equal(priority, workItem.Priority);
            Assert.Equal(WorkItemStatus.Todo, workItem.Status);
            Assert.Null(workItem.UpdatedAt);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Title_Is_Empty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new WorkItem(
                    "",
                    "Test description",
                    WorkItemPriority.Medium));
        }

        [Fact]
        public void Constructor_Should_Throw_When_Title_Exceeds_200_Characters()
        {
            // Arrange
            string title = new string('A', 201);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new WorkItem(
                    title,
                    "Test description",
                    WorkItemPriority.Medium));
        }

        [Fact]
        public void Constructor_Should_Trim_Title()
        {
            // Act
            var workItem = new WorkItem(
                "  Implement testing  ",
                null,
                WorkItemPriority.High);

            // Assert
            Assert.Equal("Implement testing", workItem.Title);
        }

        [Fact]
        public void ChangeStatus_Should_Update_Status_And_UpdatedAt()
        {
            // Arrange
            var workItem = new WorkItem(
                "Implement testing",
                null,
                WorkItemPriority.Medium);

            // Act
            workItem.ChangeStatus(WorkItemStatus.InProgress);

            // Assert
            Assert.Equal(WorkItemStatus.InProgress, workItem.Status);
            Assert.NotNull(workItem.UpdatedAt);
        }

        [Fact]
        public void ChangeStatus_Should_Throw_For_Invalid_Status()
        {
            // Arrange
            var workItem = new WorkItem(
                "Implement testing",
                null,
                WorkItemPriority.Medium);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                workItem.ChangeStatus((WorkItemStatus)999));
        }

        [Fact]
        public void Update_Should_Change_Title_Description_And_Priority()
        {
            // Arrange
            var workItem = new WorkItem(
                "Original title",
                "Original description",
                WorkItemPriority.Low);

            // Act
            workItem.Update(
                "Updated title",
                "Updated description",
                WorkItemPriority.High);

            // Assert
            Assert.Equal("Updated title", workItem.Title);
            Assert.Equal("Updated description", workItem.Description);
            Assert.Equal(WorkItemPriority.High, workItem.Priority);
            Assert.NotNull(workItem.UpdatedAt);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_Should_Reject_Invalid_Titles(string invalidTitle)
        {
            // Act
            Action action = () => new WorkItem(
                invalidTitle,
                "Testing title validation",
                WorkItemPriority.Medium);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}
