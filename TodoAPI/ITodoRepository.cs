using TodoAPI.Models;

namespace TodoAPI;

public interface ITodoRepository
{
	IEnumerable<TodoItem> GetAll();
	TodoItem? Get(int id);
	TodoItem? Add(TodoItem item);
	TodoItem? Update(TodoItem item);
	bool Delete(int id);
}
