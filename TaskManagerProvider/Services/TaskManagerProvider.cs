using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TaskManagerProvider.Contracts;

namespace TaskManagerProvider.Services;

public sealed class TaskManagerProvider
    : Contracts.TaskManagerProvider.TaskManagerProviderBase
{
    static TaskManagerProvider()
    {
        Service = new DataService();
    }

    private static readonly IDataService Service;

    public override async Task<GetUsersResponse> GetUsers(Empty request, ServerCallContext context)
    {
        var response = new GetUsersResponse();
        response.Users.AddRange(await Service.GetUsersAsync());
        
        return response;
    }

    public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var response = new GetUserByIdResponse();
        response.User = await Service.GetUserByIdAsync(request.Id);
        
        return response;
    }

    public override async Task<User> CreateUser(User user, ServerCallContext context)
    {
        var result = await Service.CreateUserAsync(user);
        return result;
    }

    public override async Task<UpdateUserResponse> UpdateUser(User user, ServerCallContext context)
    {
        var response = new UpdateUserResponse();
        response.User = await Service.UpdateUserAsync(user);

        return response;
    }

    public override async Task<RemoveUserResponse> RemoveUser(User user, ServerCallContext context)
    {
        var response = new RemoveUserResponse();
        response.IsSuccess = await Service.RemoveUserAsync(user);
        return response;
    }

    public override async Task<GetTasksResponse> GetTasks(Empty request, ServerCallContext context)
    {
        var response = new GetTasksResponse();
        response.Tasks.AddRange(await Service.GetTasksAsync());
        return response;
    }

    public override async Task<GetUserTasksResponse> GetUserTasks(GetUserTasksRequest request, ServerCallContext context)
    {
        var response = new GetUserTasksResponse();
        response.Tasks.AddRange(await Service.GetUserTasksAsync(request.Id));
        return response;
    }

    public override async Task<GetTaskByIdResponse> GetTaskById(GetTaskByIdRequest request, ServerCallContext context)
    {
        var response = new GetTaskByIdResponse();
        response.Task = await Service.GetTaskByIdAsync(request.Id);
        return response;
    }

    public override async Task<TaskItem> CreateTask(TaskItem task, ServerCallContext context)
    {
        var result = await Service.CreateTaskAsync(task);
        return result;
    }

    public override async Task<UpdateTaskResponse> UpdateTask(TaskItem task, ServerCallContext context)
    {
        var response = new UpdateTaskResponse();
        response.Task = await Service.UpdateTaskAsync(task);
        return response;
    }

    public override async Task<RemoveTaskResponse> RemoveTask(TaskItem task, ServerCallContext context)
    {
        var response = new RemoveTaskResponse();
        response.IsSuccess = await Service.RemoveTaskAsync(task);
        return response;
    }
}