using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Miksoft.QuizWithSignalR.Hub;
using Miksoft.QuizWithSignalR.Storage;

namespace Miksoft.QuizWithSignalR.Pages;
public class AdminModel : PageModel
{
    private readonly InMemoryStorage _storage;
    private readonly IHubContext<QuizHub> _hubContext;

    public List<Result> Results { get; set; } = new();

    public AdminModel(InMemoryStorage storage, IHubContext<QuizHub> hubContext)
    {
        _hubContext = hubContext;
        _storage = storage;
    }

    public void OnGet()
    {
        Results = _storage.GetResults();
    }
    
    public async Task OnPost(int questionNumber)
    {
        var question = _storage.Questions.ElementAt(questionNumber);
        var json = JsonSerializer.Serialize(question);

        await _hubContext.Clients.All.SendAsync("Question", question.Text, json);
        question.AskedOn = DateTime.Now;

        RedirectToPage("Admin");
    }

    public async Task OnPostClear()
    {
        _storage.Clear();

        RedirectToPage("Admin");
    }
}
