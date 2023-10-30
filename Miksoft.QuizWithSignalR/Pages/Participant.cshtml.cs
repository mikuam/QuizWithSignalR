using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Miksoft.QuizWithSignalR.Hub;
using Miksoft.QuizWithSignalR.Storage;

namespace Miksoft.QuizWithSignalR.Pages;
public class ParticipantModel : PageModel
{
    private readonly InMemoryStorage _storage;
    private readonly IHubContext<QuizHub> _hubContext;

    public ParticipantModel(InMemoryStorage storage, IHubContext<QuizHub> hubContext)
    {
        _hubContext = hubContext;
        _storage = storage;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        var participantName = Request.Form["Name"];
        /*
        _storage.Participants.Add(new Participant
        {
            Name = participantName,
            ConnectedOn = DateTime.Now
        });
        */
        return RedirectToPage("Game", new { participantName });
    }
}

