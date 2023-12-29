using Database.Records;

namespace Database.Entities;

public class AnswerEntity  : AnswerRecord
{
    public List<QuestionEntity> Questions { get; } = [];
    public List<QuestionAnswerEntity> QuestionAnswers { get; } = [];
}