using AutoSupportAPI.Models;
using Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AutoSupportAPI.Services;

public interface IQuestionService
{
    public Task<QuestionInfo?> GetAnswersAsync(long questionId, CancellationToken token = default);
    public Task<QuestionInfo?> GetAnswersFromRedisAsync(string questionId, CancellationToken token = default);
}

public class QuestionService : IQuestionService
{
    private readonly PostgresDbContext _pgDb;
    private readonly IDatabase _redisDb;

    public QuestionService(PostgresDbContext context)
    {
        _pgDb = context;
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("auto-cs_redis_1:6379");
        _redisDb = redis.GetDatabase();
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
                Question = record.Question,
                Answers = answers
            })
            .FirstOrDefaultAsync(token);
        return question;
    }

    public async Task<QuestionInfo?> GetAnswersFromRedisAsync(string question, CancellationToken token = default)
    {
        var values = await _redisDb.HashGetAsync("questions", question);
        return new QuestionInfo
        {
            Question = question,
            Answers = JsonConvert.DeserializeObject<List<string>>(values.ToString()) ?? new List<string>()
        };
    }
}