using TaskManager.Models;

namespace TaskManager.Services;

public interface IDataService
{
    // User operations
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(User user);

    // Task operations
    Task<List<TaskItem>> GetTasksAsync();
    Task<List<TaskItem>> GetUserTasksAsync(int userId);
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task<TaskItem?> UpdateTaskAsync(TaskItem task);
}