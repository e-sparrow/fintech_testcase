namespace TaskManager.Models;

public static class ModelExtensions
{
    public static TaskItem ToModel(this TaskManagerProvider.Contracts.TaskItem self)
    {
        var result = new TaskItem()
        {
            Id = self.Id,
            Name = self.Name,
            UserId = self.UserId,
            State = self.State.ToModel(),
            User = self.User.ToModel()
        };

        return result;
    }

    public static TaskState ToModel(this TaskManagerProvider.Contracts.TaskState self)
    {
        var result = (TaskState) self;
        return result;
    }

    public static User ToModel(this TaskManagerProvider.Contracts.User self)
    {
        var result = new User()
        {
            Id = self.Id,
            Name = self.Name
        };

        return result;
    }

    public static TaskManagerProvider.Contracts.TaskItem FromModel(this TaskItem self)
    {
        var result = new TaskManagerProvider.Contracts.TaskItem()
        {
            Id = self.Id,
            Name = self.Name,
            UserId = self.UserId,
            State = self.State.FromModel(),
            User = self.User.FromModel()
        };

        return result;
    }

    public static TaskManagerProvider.Contracts.TaskState FromModel(this TaskState self)
    {
        var result = (TaskManagerProvider.Contracts.TaskState) self;
        return result;
    }

    public static TaskManagerProvider.Contracts.User FromModel(this User self)
    {
        var result = new TaskManagerProvider.Contracts.User()
        {
            Id = self.Id,
            Name = self.Name
        };

        return result;
    }
}