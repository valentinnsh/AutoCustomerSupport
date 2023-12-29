using AutoSupportAPI.Models;
using Database;
using Microsoft.EntityFrameworkCore;

namespace AutoSupportAPI.Services;

public interface IQuestionService
{
    public Task<QuestionInfo?> GetAnswersAsync(long questionId, CancellationToken token = default);
}

public class QuestionService : IQuestionService
{
    private readonly PostgresDbContext _pgDb;

    public QuestionService(PostgresDbContext context)
    {
        _pgDb = context;
    }
    
    public async Task<QuestionInfo?> GetAnswersAsync(long questionId, CancellationToken token = default)
    {
        var answers = await _pgDb.QuestionAnswers.Where(q => q.QuestionId == questionId)
            .OrderBy(qa => qa.Rank)
            .Select(qa => qa.Answer.Answer)
            .ToListAsync(token);
        
        var question = await _pgDb.Questions.Where(q => q.Id == questionId)
            .Select(record => new QuestionInfo
            {
                Id = record.Id,
                Question = record.Question,
                Answers = answers
            })
            .FirstOrDefaultAsync(token);
        return question;
    }
}