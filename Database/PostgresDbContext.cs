using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var questions = builder.Entity<QuestionEntity>().ToTable("questions");
        questions.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Entity<AnswerEntity>().ToTable("answers").Property(p => p.Id).ValueGeneratedOnAdd();
        questions.HasMany(e => e.Answers).WithMany(e => e.Questions)
            .UsingEntity<QuestionAnswerEntity>();
        OnSeed(builder);
    }

    private void OnSeed(ModelBuilder builder)
    {
        builder.Entity<QuestionEntity>().HasData(
            new QuestionEntity
            {
                Id = 1,
                Question = "Did it work?"
            },
            new QuestionEntity
            {
                Id = 2,
                Question = "Will I finish in time?"
            });
        
        builder.Entity<AnswerEntity>().HasData(
            new AnswerEntity
            {
                Id = 1,
                Answer = "No, FU"
            },
            new AnswerEntity
            {
                Id = 2,
                Answer = "MaybeMaybe"
            },
            new AnswerEntity
            {
                Id = 3,
                Answer = "MoreAnswers"
            },
            new AnswerEntity
            {
                Id = 4,
                Answer = "MoreAnswers 2"
            });

        builder.Entity<QuestionAnswerEntity>().HasData(
            new QuestionAnswerEntity
            {
                Id = 1,
                QuestionId = 1,
                AnswerId = 1,
                Rank = 2
            },
            new QuestionAnswerEntity
            {
                Id = 2,
                QuestionId = 1,
                AnswerId = 2,
                Rank = 1
            },
            new QuestionAnswerEntity
            {
                Id = 3,
                QuestionId = 2,
                AnswerId = 2,
                Rank = 2
            },
            new QuestionAnswerEntity
            {
                Id = 4,
                QuestionId = 2,
                AnswerId = 4,
                Rank = 1
            });
    }
    
    public IQueryable<QuestionEntity> Questions => Set<QuestionEntity>();
    public IQueryable<QuestionAnswerEntity> QuestionAnswers => Set<QuestionAnswerEntity>();
    public IQueryable<AnswerEntity> Answers => Set<AnswerEntity>();

}
