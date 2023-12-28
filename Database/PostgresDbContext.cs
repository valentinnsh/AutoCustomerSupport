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

        OnSeed(builder);
    }

    private void OnSeed(ModelBuilder builder)
    {
        builder.Entity<QuestionEntity>().HasData(
            new QuestionEntity
            {
                Id = 1,
                Question = "Did it work?"
            });
    }
    
    public IQueryable<QuestionEntity> Questions => Set<QuestionEntity>();
}
