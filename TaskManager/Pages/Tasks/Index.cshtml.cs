using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Tasks;

public class IndexModel : PageModel
{
    private readonly IDataService _dataService;
    public List<TaskItem> Tasks { get; set; } = new();
    public SelectList Users { get; set; } = default!;
    public int? SelectedUserId { get; set; }

    public IndexModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task OnGetAsync(int? userId)
    {
        var users = await _dataService.GetUsersAsync();
        Users = new SelectList(users, "Id", "Name", userId);  // Pass selected value to maintain selection
        SelectedUserId = userId;

        // Handle filtering
        if (userId.HasValue)
        {
            Tasks = await _dataService.GetUserTasksAsync(userId.Value);
        }
        else
        {
            Tasks = await _dataService.GetTasksAsync();
        }
    }
}