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
    }
    
    public IQueryable<QuestionEntity> Questions => Set<QuestionEntity>();
    public IQueryable<QuestionAnswerEntity> QuestionAnswers => Set<QuestionAnswerEntity>();
    public IQueryable<AnswerEntity> Answers => Set<AnswerEntity>();

}
