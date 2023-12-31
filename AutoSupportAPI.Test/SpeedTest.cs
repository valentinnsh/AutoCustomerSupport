using System.Text;
using AutoSupportAPI.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AutoSupportAPI.Test;

[TestFixture]
public class SpeedTest
{
    private HttpClient _client;

    [SetUp]
    public void SetUp()
    {
        _client = new HttpClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public async Task SimpleSpeedTest()
    {
        var url = "http://localhost:8080/api/questions/4";

        var start = DateTime.Now;
        var response = await _client.GetAsync(url);
        var delta = DateTime.Now - start;
        Console.WriteLine($"PG: {delta.Milliseconds}");

        start = DateTime.Now;
        response = await _client.GetAsync("http://localhost:8080/api/questions/redis/What%20is%20next%20to%20the%20Main%20Building%3F");
        delta = DateTime.Now - start;
        Console.WriteLine($"Redis: {delta.Milliseconds}");
    }

    [Test]
    public async Task TestEndpointSpeed()
    {
        var pgDat = new List<int>();
        var redisDat = new List<int>();
        
        for (int i = 0; i < 10; i++)
        {
            var (pg, redis) = await TestEndpoints(16);
            pgDat.Add(pg.Milliseconds);
            redisDat.Add(redis.Milliseconds);
        }
        
        Console.WriteLine($"PG: {pgDat.Average()}\nRedis:{redisDat.Average()}");
    }

    private async Task<(TimeSpan, TimeSpan)> TestEndpoints(int taskNum)
    {
        var random = new Random();
        var searchIds = Enumerable.Range(0, taskNum)
            .Select(i => random.Next(1, 1000 + 1))
            .ToArray();

        var sql_urls = searchIds.Select(_ => "http://localhost:8080/api/questions/"+_.ToString()).ToList();
        var tasks = sql_urls.Select(url => _client.GetAsync(url));

        var start = DateTime.Now;
        await Task.WhenAll(tasks);
        var pgDelta = DateTime.Now - start;
        
        var results = tasks.Select(t =>JsonConvert.DeserializeObject<QuestionInfo>(t.Result.Content.ReadAsStringAsync().Result)).ToList();
        var qsc = results.Select(v => v.ToString().Replace(" ", "%20").Replace("?", "%3F")).ToList();
        tasks = qsc.Select(q => _client.GetAsync("http://localhost:8080/api/questions/redis/"+q)).ToList();
        start = DateTime.Now;
        await Task.WhenAll(tasks);
        var redisDelta = DateTime.Now - start;
        return (pgDelta, redisDelta);
    }
}