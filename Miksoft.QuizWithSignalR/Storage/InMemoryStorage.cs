using System.Text.Json.Serialization;

namespace Miksoft.QuizWithSignalR.Storage;

public class Result
{
    public int CorrectAnswers { get; set; }
    public TimeSpan TotalTime { get; set; }
    public string ParticipantName { get; set; }
}

public class Participant
{
    public string ConnectionId { get; set; }
    public string Name { get; set; }
    public DateTime ConnectedOn { get; set; }
    public List<Answer> Answers { get; set; } = new List<Answer>();
}

public class Answer
{
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
            Text = "What is the capital of Poland?",
            A = "Warsaw",
            B = "Cracow",
            C = "Gdansk",
            D = "Wroclaw",
            CorrectAnswer = "A"
        });
    }

    public List<Participant> Participants { get; set; } = new List<Participant>();
    public List<Question> Questions { get; set; } = new List<Question>();
}
