using Microsoft.AspNetCore.Mvc.RazorPages;
using Miksoft.QuizWithSignalR.Storage;

namespace Miksoft.QuizWithSignalR.Pages;
public class IndexModel : PageModel
{
    private readonly InMemoryStorage _storage;

    public List<Result> Results { get; set; } = new();

    public IndexModel(InMemoryStorage storage)
    {
        _storage = storage;
    }

    public void OnGet()
    {
        Results = _storage.GetResults();
    }
}
