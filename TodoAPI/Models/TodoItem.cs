using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models;

public class TodoItem
{
	public string Name { get; set; } = default!;
	public int Priority { get; set; }
	public Status Status { get; set; }
}

public enum Status
{
	[Display(Name = "Not started")]
	NotStarted,
	[Display(Name = "In progress")]
	InProgress,
	[Display(Name = "Completed")]
	Completed
}