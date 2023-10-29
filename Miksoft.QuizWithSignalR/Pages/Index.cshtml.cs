using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Miksoft.QuizWithSignalR.Hub;
using Miksoft.QuizWithSignalR.Storage;

namespace Miksoft.QuizWithSignalR.Pages;
public class IndexModel : PageModel
{
    private readonly InMemoryStorage _storage;
    private readonly IHubContext<QuizHub> _hubContext;

    public List<Result> Results { get; set; } = new();

    public IndexModel(InMemoryStorage storage, IHubContext<QuizHub> hubContext)
    {
        _hubContext = hubContext;
        _storage = storage;
    }

    public void OnGet()
    {
        Results = CalculateResults(_storage);
    }
    
    public async Task OnPostSendQuestion1()
    {
        var question = _storage.Questions[0];
        var json = JsonSerializer.Serialize(question);

        await _hubContext.Clients.All.SendAsync("Question", "Question 1", json);
        question.AskedOn = DateTime.Now;
    }

    private List<Result> CalculateResults(InMemoryStorage storage)
    {
        var results = new List<Result>();

        foreach (var participant in storage.Participants)
        {
            var correctAnswers = participant.Answers.Count(x => x.IsCorrect);
            var totalTime = participant.Answers.Sum(x => x.AnswerTime.Ticks);
            var result = new Result
            {
                CorrectAnswers = correctAnswers,
                TotalTime = TimeSpan.FromTicks(totalTime),
                ParticipantName = participant.Name
            };
            results.Add(result);
        }

        return results.OrderByDescending(x => x.CorrectAnswers).ThenBy(x => x.TotalTime).ToList();
    }
}
