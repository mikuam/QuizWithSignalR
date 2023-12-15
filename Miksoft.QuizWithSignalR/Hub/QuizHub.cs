using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Miksoft.QuizWithSignalR.Storage;

namespace Miksoft.QuizWithSignalR.Hub;

public class QuizHub : Microsoft.AspNetCore.SignalR.Hub
{
    public const string EndpointPath = "/quizHub";

    private readonly IHubContext<QuizHub> _hubContext;
    private readonly InMemoryStorage _storage;

    private IClientProxy Client => _hubContext.Clients.Client(Context.ConnectionId);

    public QuizHub(IHubContext<QuizHub> hubContext, InMemoryStorage storage)
    {
        _storage = storage;
        _hubContext = hubContext;
    }

    public async Task Register(string connectionId, string name)
    {
        _storage.Participants.Add(new Participant
        {
            Name = name,
            ConnectedOn = DateTime.Now,
            ConnectionId = connectionId
        });

        await Client.SendAsync("ReceiveMessage", connectionId, "Registered, thanks!!");
        await _hubContext.Clients.All.SendAsync("ParticipantRegistered", connectionId, name);
    }

    public async Task SubmitAnswer(string connectionId, string message)
    {
        var participant = _storage.Participants.FirstOrDefault(x => x.ConnectionId == connectionId);
        if (participant == null)
        {
            await Client.SendAsync("ReceiveMessage", connectionId, "You are not registered!");
            return;
        }

        var answerRequest = JsonSerializer.Deserialize<AnswerRequest>(message);
        var question = _storage.Questions.FirstOrDefault(x => x.Text == answerRequest.Question);

        if (AnswerAlreadyExists(participant, question)) return;
        
        var answer = new Answer
        {
            Question = question.Text,
            AnswerTime = DateTime.Now - question.AskedOn,
            IsCorrect = answerRequest.Answer == question.CorrectAnswer
        };

        participant.Answers.Add(answer);

        await Client.SendAsync("ReceiveMessage", connectionId, "Answer submitted, thanks!");
        
        await _hubContext.Clients.All.SendAsync("LeaderboardUpdated", participant.ConnectionId, JsonSerializer.Serialize(participant.GetResult()));
    }

    private bool AnswerAlreadyExists(Participant participant, Question question)
    {
        return participant.Answers.Any(x => x.Question == question.Text);
    }
}

