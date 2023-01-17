using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models;

public record class TodoItem
{
	public int Id { get; set; }

	[Required(AllowEmptyStrings = false)]
	[DisplayFormat(ConvertEmptyStringToNull = false)]
	public string Name { get; init; } = default!;

	[Required]
	public int Priority { get; init; }

	[Required]
	public Status Status { get; init; }

	public TodoItem(int id, string name, int priority, Status status)
	{
		Id = id;
		Name = name;
		Priority = priority;
		Status = status;
	}
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