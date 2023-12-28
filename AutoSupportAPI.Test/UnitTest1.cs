using Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace AutoSupportAPI.Test;

public class Tests
{
    private IConfigurationBuilder _config;

    [SetUp]
    public void Setup()
    {
        _config = new ConfigurationBuilder();
        var data = new Dictionary<string, string> { { "ConnectionStrings:PostgresConnection", "Host=auto-cs_postgres-db; Port=5432; Database=pgtest; Username=pgtest; Password=pgtest; Pooling=true;" } };
        _config.AddInMemoryCollection(data);
    }

    [Test]
    public async Task Test1()
    {
        /*var context = new PostgresDbContext();
        var test = await context.Questions.FirstOrDefaultAsync();
        test.Should().BeNull();*/
    }
}