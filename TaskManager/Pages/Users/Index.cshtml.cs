using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Users;

public class IndexModel : PageModel
{
    private readonly IDataService _dataService;
    public List<User> Users { get; set; } = new();

    public IndexModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task OnGetAsync()
    {
        Users = await _dataService.GetUsersAsync();
    }
}