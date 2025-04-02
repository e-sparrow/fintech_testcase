using System.Collections.Concurrent;
using TaskManager.Models;

namespace TaskManager.Services;

public class DataService : IDataService
{
    private readonly ConcurrentDictionary<int, User> _users;
    private readonly ConcurrentDictionary<int, TaskItem> _tasks;
    private int _lastUserId = 0;
    private int _lastTaskId = 0;

    public DataService()
    {
        _users = new ConcurrentDictionary<int, User>();
        _tasks = new ConcurrentDictionary<int, TaskItem>();

        // Add some mock data
        var user1 = new User { Id = ++_lastUserId, Name = "John Doe" };
        var user2 = new User { Id = ++_lastUserId, Name = "Jane Smith" };

        _users.TryAdd(user1.Id, user1);
        _users.TryAdd(user2.Id, user2);

        var task1 = new TaskItem { Id = ++_lastTaskId, Name = "Complete project", UserId = user1.Id, State = TaskState.New };
        var task2 = new TaskItem { Id = ++_lastTaskId, Name = "Review code", UserId = user1.Id, State = TaskState.InProgress };
        var task3 = new TaskItem { Id = ++_lastTaskId, Name = "Write documentation", UserId = user2.Id, State = TaskState.New };

        _tasks.TryAdd(task1.Id, task1);
        _tasks.TryAdd(task2.Id, task2);
        _tasks.TryAdd(task3.Id, task3);
    }

    public Task<List<User>> GetUsersAsync()
    {
        return Task.FromResult(_users.Values.ToList());
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<User> CreateUserAsync(User user)
    {
        user.Id = Interlocked.Increment(ref _lastUserId);
        if (_users.TryAdd(user.Id, user))
        {
            return Task.FromResult(user);
        }
        throw new InvalidOperationException("Failed to create user");
    }

    public Task<User?> UpdateUserAsync(User user)
    {
        if (_users.ContainsKey(user.Id))
        {
            _users[user.Id] = user;
            return Task.FromResult<User?>(user);
        }
        return Task.FromResult<User?>(null);
    }

    public Task<List<TaskItem>> GetTasksAsync()
    {
        var tasks = _tasks.Values.ToList();
        foreach (var task in tasks)
        {
            _users.TryGetValue(task.UserId, out var user);
            task.User = user;
        }
        return Task.FromResult(tasks);
    }

    public Task<List<TaskItem>> GetUserTasksAsync(int userId)
    {
        // Verify user exists first
        if (!_users.ContainsKey(userId))
        {
            return Task.FromResult(new List<TaskItem>());
        }

        var user = _users[userId];
        // First filter the tasks by userId, then materialize to list
        var filteredTasks = _tasks.Values
            .Where(t => t.UserId == userId)
            .ToList();

        // Ensure all tasks have their User property set
        foreach (var task in filteredTasks)
        {
            task.User = user;
        }

        return Task.FromResult(filteredTasks);
    }

    public Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        if (_tasks.TryGetValue(id, out var task))
        {
            _users.TryGetValue(task.UserId, out var user);
            task.User = user;
            return Task.FromResult<TaskItem?>(task);
        }
        return Task.FromResult<TaskItem?>(null);
    }

    public Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        task.Id = Interlocked.Increment(ref _lastTaskId);
        if (_tasks.TryAdd(task.Id, task))
        {
            _users.TryGetValue(task.UserId, out var user);
            task.User = user;
            return Task.FromResult(task);
        }
        throw new InvalidOperationException("Failed to create task");
    }

    public Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        if (_tasks.ContainsKey(task.Id))
        {
            // Ensure we have a valid user
            if (_users.TryGetValue(task.UserId, out var user))
            {
                task.User = user;
                _tasks[task.Id] = task;
                return Task.FromResult<TaskItem?>(task);
            }
            // If user doesn't exist, don't update
            return Task.FromResult<TaskItem?>(null);
        }
        return Task.FromResult<TaskItem?>(null);
    }
}