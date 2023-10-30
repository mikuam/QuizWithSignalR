using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace Miksoft.QuizWithSignalR.Storage;

public class Result
{
    public string ConnectionId { get; set; }
    public int AnswersCount { get; set; }
    public int CorrectAnswers { get; set; }
    public TimeSpan TotalTime { get; set; }
    public string ParticipantName { get; set; }
}

public class Participant
{
    public string ConnectionId { get; set; }
    public string Name { get; set; }
    public DateTime ConnectedOn { get; set; }
    public ConcurrentBag<Answer> Answers { get; set; } = new ConcurrentBag<Answer>();

    public Result GetResult()
    {
        var correctAnswers = Answers.Count(x => x.IsCorrect);
        var totalTime = Answers.Sum(x => x.AnswerTime.Ticks);
        var result = new Result
        {
            ConnectionId = ConnectionId,
            AnswersCount = Answers.Count,
            CorrectAnswers = correctAnswers,
            TotalTime = TimeSpan.FromTicks(totalTime),
            ParticipantName = Name
        };
        return result;
    }
}

public class Answer
{
    public string Question { get; set; }
    public bool IsCorrect { get; set; }
    public TimeSpan AnswerTime { get; set; }
}

public class AnswerRequest
{
    public string Question { get; set; }
    public string Answer { get; set; }
}

public class Question
{
    public string Text { get; set; }
    public string A { get; set; }
    public string B { get; set; }
    public string C { get; set; }
    public string D { get; set; }

    [JsonIgnore]
    public string CorrectAnswer { get; set; }

    [JsonIgnore]
    public DateTime AskedOn { get; set; }
}

public class InMemoryStorage
{
    public InMemoryStorage()
    {
        Questions.Add(new Question
        {
            Text = "What communication protocol SignalR is NOT using",
            A = "Web Sockets API",
            B = "WebRTC",
            C = "HTML 5 API SSE",
            D = "Comet",
            CorrectAnswer = "B"
        });
        Questions.Add(new Question
        {
            Text = "What are scenarios where SignalR would NOT proof useful",
            A = "Notifications, alerts",
            B = "Chat",
            C = "Fetching data on the page",
            D = "Dashboards, constant updates",
            CorrectAnswer = "C"
        });
        Questions.Add(new Question
        {
            Text = "SignalR is a communication",
            A = "From Server to Client",
            B = "From Client to Server",
            C = "Both ways",
            D = "",
            CorrectAnswer = "C"
        });
        Questions.Add(new Question
        {
            Text = "What is the capital of Poland?",
            A = "Warsaw",
            B = "Cracow",
            C = "Gdansk",
            D = "Wroclaw",
            CorrectAnswer = "A"
        });
    }

    public ConcurrentBag<Participant> Participants { get; set; } = new ConcurrentBag<Participant>();
    public ConcurrentBag<Question> Questions { get; set; } = new ConcurrentBag<Question>();

    public List<Result> GetResults()
    {
        return Participants
            .Select(participant => participant.GetResult())
            .OrderByDescending(x => x.CorrectAnswers)
            .ThenBy(x => x.TotalTime)
            .ToList();
    }
}
