using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using TaskManager.Models;
using TaskManagerProvider.Contracts;
using TaskItem = TaskManager.Models.TaskItem;
using User = TaskManager.Models.User;
using TaskManagerProviderClient = TaskManagerProvider.Contracts.TaskManagerProvider.TaskManagerProviderClient;
using TaskState = TaskManager.Models.TaskState;

namespace TaskManager.Services;

public class DataService 
    : IDataService
{
    private const string Address = "http://localhost:50051";
    
    public async Task<List<User>> GetUsersAsync()
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        var reply = await client.GetUsersAsync(new Empty());
        return reply.Users.Select(value => value.ToModel()).ToList();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var request = new GetUserByIdRequest()
        {
            Id = id
        };
        
        var reply = await client.GetUserByIdAsync(request);
        return reply.User.ToModel();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var reply = await client.CreateUserAsync(user.FromModel());
        return reply.ToModel();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var reply = await client.UpdateUserAsync(user.FromModel());
        return reply.User.ToModel();
    }

    public async Task<bool> RemoveUserAsync(int id)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);

        var fakeUser = new User()
        {
            Id = id,
            Name = string.Empty
        };
        
        var reply = await client.RemoveUserAsync(fakeUser.FromModel());
        return reply.IsSuccess;
    }

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var reply = await client.GetTasksAsync(new Empty());
        return reply.Tasks.Select(value => value.ToModel()).ToList();
    }

    public async Task<List<TaskItem>> GetUserTasksAsync(int id)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var request = new GetUserTasksRequest()
        {
            Id = id
        };
        
        var reply = await client.GetUserTasksAsync(request);
        return reply.Tasks.Select(value => value.ToModel()).ToList();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var request = new GetTaskByIdRequest()
        {
            Id = id
        };
        
        var reply = await client.GetTaskByIdAsync(request);
        return reply.Task.ToModel();
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var reply = await client.CreateTaskAsync(task.FromModel());
        return reply.ToModel();
    }

    public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);
        
        var reply = await client.UpdateTaskAsync(task.FromModel());
        return reply.Task.ToModel();
    }

    public async Task<bool> RemoveTaskAsync(int id)
    {
        using var channel = CreateChannel();
        var client = new TaskManagerProviderClient(channel);

        var fakeTask = new TaskItem()
        {
            Id = id,
            Name = string.Empty,
            UserId = -1,
            State = TaskState.Close,
            User = new User()
            {
                Id = -1,
                Name = string.Empty
            }
        };
        
        var reply = await client.RemoveTaskAsync(fakeTask.FromModel());
        return reply.IsSuccess;
    }

    private static GrpcChannel CreateChannel()
    {
        var result = GrpcChannel.ForAddress(Address);
        return result;
    }
}