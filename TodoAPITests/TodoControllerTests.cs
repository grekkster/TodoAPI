using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoAPI.Controllers;
using TodoAPI.Models;

namespace TodoAPI.Tests;

public class TodoControllerTests
{
    private readonly Mock<ITodoRepository> todoRepositoryMock = new();
    
	private readonly List<TodoItem> initialTodos = new()
	{
		new TodoItem(0, "Update CV", 1, Status.Completed),
		new TodoItem(1, "Learn React", 5, Status.InProgress)
};

	[Fact]
    public void Get()
    {
        // Arrange
        todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
        var controller = new TodoController(todoRepositoryMock.Object);

        // Act
        var result = controller.Get();

        // Assert
		OkObjectResult objectResult = Assert.IsType<OkObjectResult>(result.Result);
        IEnumerable<TodoItem> resultTodos = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(objectResult.Value);
		Assert.Equal(initialTodos, resultTodos);
    }

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public void GetById(int id)
	{
		// Arrange
		todoRepositoryMock.Setup(m => m.Get(id)).Returns(initialTodos[id]);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Get(id);

		// Assert
		OkObjectResult objectResult = Assert.IsType<OkObjectResult>(result.Result);
		TodoItem resultTodo = Assert.IsAssignableFrom<TodoItem>(objectResult.Value);
		Assert.Equal(initialTodos[id], resultTodo);
	}

	[Fact]
	public void GetById_NotFound()
	{
		// Arrange
		var nonExistentId = 0;
		todoRepositoryMock.Setup(m => m.Get(nonExistentId)).Returns<TodoItem?>(null);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Get(nonExistentId);

		// Assert
		NotFoundResult notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
	}

	[Fact]
	public void Post()
	{
		// Arrange
		var newTodo = new TodoItem(0, "Another todo", 1, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		todoRepositoryMock.Setup(m => m.Add(newTodo)).Returns(newTodo);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Post(newTodo);

		// Assert
		CreatedAtActionResult objectResult = Assert.IsType<CreatedAtActionResult>(result.Result);
		TodoItem resultTodo = Assert.IsAssignableFrom<TodoItem>(objectResult.Value);
		Assert.Equal(newTodo, resultTodo);
	}

	[Fact]
	public void Post_ModelInvalid_SameName()
	{
		// Arrange
		var newTodo = new TodoItem(0, initialTodos[1].Name, 1, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Post(newTodo);

		// Assert
		BadRequestObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
	}

	[Fact]
	public void Post_AddReturnsNull()
	{
		// Arrange
		var newTodo = new TodoItem(0, "Another todo", 1, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		todoRepositoryMock.Setup(m => m.Add(newTodo)).Returns<TodoItem?>(null);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Post(newTodo);

		// Assert
		ConflictResult objectResult = Assert.IsType<ConflictResult>(result.Result);
	}

	[Fact]
	public void Put()
	{
		// Arrange
		var updatedTodo = new TodoItem(0, "Update CV", 2, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		todoRepositoryMock.Setup(m => m.Update(updatedTodo)).Returns(updatedTodo);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Put(updatedTodo.Id, updatedTodo);

		// Assert
		OkObjectResult objectResult = Assert.IsType<OkObjectResult>(result.Result);
		TodoItem resultTodo = Assert.IsAssignableFrom<TodoItem>(objectResult.Value);
		Assert.Equal(updatedTodo, resultTodo);
	}

	[Fact]
	public void Put_ModelInvalid_IdDoesNotMatch()
	{
		// Arrange
		var updatedTodo = new TodoItem(0, initialTodos[1].Name, 1, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Put(updatedTodo.Id + 1, updatedTodo);

		// Assert
		BadRequestObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
	}

	[Fact]
	public void Put_ModelInvalid_SameName()
	{
		// Arrange
		var updatedTodo = new TodoItem(0, initialTodos[1].Name, 1, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Put(updatedTodo.Id, updatedTodo);

		// Assert
		BadRequestObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
	}

	[Fact]
	public void Put_UpdateReturnsNull()
	{
		// Arrange
		var updatedTodo = new TodoItem(0, "Another todo", 1, Status.NotStarted);
		todoRepositoryMock.Setup(m => m.GetAll()).Returns(initialTodos);
		todoRepositoryMock.Setup(m => m.Update(updatedTodo)).Returns<TodoItem?>(null);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Put(updatedTodo.Id, updatedTodo);

		// Assert
		NotFoundResult notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
	}

	[Fact]
	public void Delete()
	{
		// Arrange
		todoRepositoryMock.Setup(m => m.Get(0)).Returns(initialTodos[0]);
		todoRepositoryMock.Setup(m => m.Delete(0)).Returns(true);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Delete(0);

		// Assert
		NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
	}

	[Fact]
	public void Delete_NotFound()
	{
		// Arrange
		todoRepositoryMock.Setup(m => m.Get(2)).Returns<TodoItem?>(null);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Delete(2);

		// Assert
		NotFoundResult notFoundResult = Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public void Delete_NotCompleted()
	{
		// Arrange
		todoRepositoryMock.Setup(m => m.Get(1)).Returns(initialTodos[1]);
		var controller = new TodoController(todoRepositoryMock.Object);

		// Act
		var result = controller.Delete(1);

		// Assert
		BadRequestObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result);
	}
}
