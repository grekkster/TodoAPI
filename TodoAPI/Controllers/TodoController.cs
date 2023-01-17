using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
	private readonly ITodoRepository todoRepository;

	public TodoController(ITodoRepository todoRepository)
	{
		this.todoRepository = todoRepository;
	}

	// GET: api/<TodoController>
	[HttpGet]
	public ActionResult<IEnumerable<TodoItem>> Get()
	{
		return Ok(todoRepository.GetAll());
	}

	// GET api/<TodoController>/5
	[HttpGet("{id}")]
	public ActionResult<TodoItem> Get(int id)
	{
		var result = todoRepository.Get(id);
		return result is null ? NotFound() : Ok(result);
	}

	// POST api/<TodoController>
	[HttpPost]
	public ActionResult<TodoItem> Post([FromBody] TodoItem newTodo)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		if (todoRepository.GetAll().Any(x => x.Name == newTodo.Name))
		{
			ModelState.AddModelError(nameof(newTodo.Name), $"Task with name: '{newTodo.Name}' already exists.");
			return BadRequest(ModelState);
		}

		var result = todoRepository.Add(newTodo);
		return result is null ? Conflict() : CreatedAtAction(nameof(Get), new { result.Id }, result);
	}

	// PUT api/<TodoController>/5
	[HttpPut("{id}")]
	public ActionResult<TodoItem> Put(int id, [FromBody] TodoItem todo)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		if (id != todo.Id)
		{
			ModelState.AddModelError(nameof(id), "Query id and Todo id does not match.");
			return BadRequest(ModelState);
		}

		if (todoRepository.GetAll().Any(x => x.Name == todo.Name && x.Id != id))
		{
			ModelState.AddModelError(nameof(todo.Name), $"Task with name: '{todo.Name}' already exists.");
			return BadRequest(ModelState);
		}

		var result = todoRepository.Update(todo);

		return result is null ? NotFound() : Ok(result);
	}

	// DELETE api/<TodoController>/5
	[HttpDelete("{id}")]
	public ActionResult Delete(int id)
	{
		var todo = todoRepository.Get(id);
		if (todo is null) return NotFound();
		if (todo.Status != Status.Completed) return BadRequest("Unable to delete unfinished task.");
		
		todoRepository.Delete(id);
		return NoContent();
	}
}
