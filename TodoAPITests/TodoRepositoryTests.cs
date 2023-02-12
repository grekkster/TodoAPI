using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPITests;

public class TodoRepositoryTests
{
	private readonly TodoRepository todoRepository = new();

	[Fact]
	public void Add()
	{
		// Arrange
		var newTodo = new TodoItem(id: 0, name: "test", priority: 6, status: Status.Completed);
		var count = todoRepository.GetAll().Count();

		// Act
		todoRepository.Add(newTodo);

		// Assert
		Assert.Equal(count + 1, todoRepository.GetAll().Count());
		Assert.Equal(newTodo, todoRepository.Get(2));
	}

	[Fact]
	public void Delete()
	{
		// Arrange
		var count = todoRepository.GetAll().Count();
		Assert.True(todoRepository.Get(0) is not null);

		// Act
		var result = todoRepository.Delete(id: 0);

		// Assert
		Assert.True(result);
		Assert.Equal(count - 1, todoRepository.GetAll().Count());
		Assert.True(todoRepository.Get(0) is null);
	}

	[Fact]
	public void Delete_NotExist()
	{
		// Arrange
		var count = todoRepository.GetAll().Count();

		// Act
		var result = todoRepository.Delete(id: 3);

		// Assert
		Assert.False(result);
		Assert.Equal(count, todoRepository.GetAll().Count());
	}

	[Fact]
	public void Get()
	{
		// Arrange
		var expected = new TodoItem(id: 1, name: "Learn React", priority: 5, status: Status.InProgress);

		// Act
		var result = todoRepository.Get(id: 1);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void Get_NotExist()
	{
		// Act
		var result = todoRepository.Get(id: 3);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void GetAll()
	{
		// Arrange
		var first = new TodoItem(0, "Update CV", 1, Status.Completed);
		var second = new TodoItem(1, "Learn React", 5, Status.InProgress);

		// Act
		var result = todoRepository.GetAll().ToList();

		// Assert
		Assert.Equal(2, result.Count);
		Assert.Equal(first, result[0]);
		Assert.Equal(second, result[1]);
	}

	[Fact]
	public void Update()
	{
		// Arrange
		var updatedTodo = todoRepository.Get(0)! with { Priority = 2, Status = Status.Completed };

		// Act
		var result = todoRepository.Update(updatedTodo);

		// Assert
		Assert.Equal(updatedTodo, todoRepository.Get(0));
		Assert.Equal(updatedTodo, result);
	}

	[Fact]
	public void Update_NotExist()
	{
		// Arrange
		var nonExistentTodo = todoRepository.Get(0)! with { Id = 3 };

		// Act
		var result = todoRepository.Update(nonExistentTodo);

		// Assert
		Assert.Null(result);
	}
}