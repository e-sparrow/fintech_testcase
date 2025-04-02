namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int UserId { get; set; }
    public TaskState State { get; set; }

    // Navigation property
    public User? User { get; set; }
}