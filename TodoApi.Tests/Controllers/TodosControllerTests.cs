using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Exceptions;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests.Controllers
{ 
    public class TodosControllerTests
    {
        [Fact]
        public async Task GetAll()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.GetAllTodosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TodoResponse> { new() { Id = 1, Title = "ABC" } });

            var controller = new TodosController(serviceMock.Object);
            var result = await controller.GetAll(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var todos = Assert.IsAssignableFrom<List<TodoResponse>>(okResult.Value);
            Assert.Single(todos);
        }

        [Fact]
        public async Task GetById_ExistingId()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.GetTodoByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TodoResponse { Id = 1, Title = "ABC" });

            var controller = new TodosController(serviceMock.Object);
            var result = await controller.GetById(1, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var todo = Assert.IsType<TodoResponse>(okResult.Value);
            Assert.Equal("ABC", todo.Title);
        }

        [Fact]
        public async Task GetById_MissingId()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.GetTodoByIdAsync(999, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TodoNotFoundException(999));

            var controller = new TodosController(serviceMock.Object);

            await Assert.ThrowsAsync<TodoNotFoundException>(() => controller.GetById(999, CancellationToken.None));
        }

        [Fact]
        public async Task Create_ValidRequest_Created()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.CreateTodoAsync(It.IsAny<CreateTodoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TodoResponse { Id = 1, Title = "Data" });

            var controller = new TodosController(serviceMock.Object);
            var result = await controller.Create(new CreateTodoRequest { Title = "New" }, CancellationToken.None);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(TodosController.GetById), createdResult.ActionName);
            var todo = Assert.IsType<TodoResponse>(createdResult.Value);
            Assert.Equal(1, todo.Id);
        }

        [Fact]
        public async Task Update_ExistingId()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.UpdateTodoAsync(1, It.IsAny<UpdateTodoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TodoResponse { Id = 1, Title = "Updated" });

            var controller = new TodosController(serviceMock.Object);
            var result = await controller.Update(1, new UpdateTodoRequest { Title = "Updated" }, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var todo = Assert.IsType<TodoResponse>(okResult.Value);
            Assert.Equal("Updated", todo.Title);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsNoContent()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.DeleteTodoAsync(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new TodosController(serviceMock.Object);
            var result = await controller.Delete(1, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_MissingId()
        {
            var serviceMock = new Mock<ITodoService>();
            serviceMock.Setup(s => s.DeleteTodoAsync(111, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TodoNotFoundException(111));

            var controller = new TodosController(serviceMock.Object);

            await Assert.ThrowsAsync<TodoNotFoundException>(() => controller.Delete(111, CancellationToken.None));
        }
    }
}
