using Database.Records;

namespace Database.Entities;

public class QuestionEntity : QuestionRecord
{
    public List<AnswerEntity> Answers { get; } = [];
    public List<QuestionAnswerEntity> QuestionAnswers { get; } = [];
}