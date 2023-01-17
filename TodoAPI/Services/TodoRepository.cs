using System.Collections.Concurrent;
using TodoAPI.Models;

namespace TodoAPI.Services;

public class TodoRepository : ITodoRepository
{
	private readonly ConcurrentDictionary<int, TodoItem> todos = new();

	public TodoRepository()
	{
		todos.TryAdd(0, new TodoItem(0, "Update CV", 1, Status.Completed));
		todos.TryAdd(1, new TodoItem(1, "Learn React", 5, Status.InProgress));
	}

	/// <summary>
	/// Adds new <see cref="TodoItem"/> to repository.
	/// Initial <see cref="TodoItem.Id"/> value is ignored, Id is updated to first value after maximum existing id in repository.
	/// </summary>
	/// <param name="item">New <see cref="TodoItem"/>.</param>
	/// <returns>Added <see cref="TodoItem"/> if successfuly added; <see langword="null"/> otherwise.</returns>
	public TodoItem? Add(TodoItem item)
	{
		item.Id = todos.Keys.Max() + 1;
		return todos.TryAdd(item.Id, item) ? item : null;
	}

	/// <summary>
	/// Delete specified <see cref="TodoItem"/>.
	/// </summary>
	/// <param name="id">Id of <see cref="TodoItem"/> to be deleted.</param>
	/// <returns><see langword="true"/> if an object was removed successfully; <see langword="false"/> otherwise.</returns>
	public bool Delete(int id)
	{
		return todos.TryRemove(id, out var _);
	}

	/// <summary>
	/// Gets specified <see cref="TodoItem"/>.
	/// </summary>
	/// <param name="id">Id of <see cref="TodoItem"/> to get.</param>
	/// <returns>Specified <see cref="TodoItem"/> if found; <see langword="null"/> otherwise.</returns>
	public TodoItem? Get(int id)
	{
		return todos.TryGetValue(id, out var item) ? item : null;
	}

	/// <summary>
	/// Gets all <see cref="TodoItem"/>s in repository.
	/// </summary>
	/// <returns>All <see cref="TodoItem"/>s in repository</returns>
	public IEnumerable<TodoItem> GetAll()
	{
		return todos.Values;
	}

	/// <summary>
	/// Updates <see cref="TodoItem"/>.
	/// </summary>
	/// <param name="item"><see cref="TodoItem"/> to update to.</param>
	/// <returns>Updated <see cref="TodoItem"/> if found; <see langword="null"/> otherwise.</returns>
	public TodoItem? Update(TodoItem item)
	{
		if (!todos.ContainsKey(item.Id))
		{
			return null;
		}
		todos[item.Id] = item;
		return item;

	}
}
