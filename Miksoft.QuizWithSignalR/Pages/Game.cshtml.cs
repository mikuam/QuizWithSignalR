using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miksoft.QuizWithSignalR.Storage;

namespace Miksoft.QuizWithSignalR.Pages;
public class GameModel : PageModel
{
    private readonly InMemoryStorage _storage;

    public GameModel(InMemoryStorage storage)
    {
        _storage = storage;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return StatusCode(200);
    }
}

