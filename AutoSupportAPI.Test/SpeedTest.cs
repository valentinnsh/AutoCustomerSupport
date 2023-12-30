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
        // Arrange
        var url = "http://localhost:8080/api/questions/4";
        
        // Act
        var start = DateTime.Now;
        var response = await _client.GetAsync(url);
        var delta = DateTime.Now - start;
        Console.WriteLine($"PG: {delta.Milliseconds}");

        start = DateTime.Now;
        response = await _client.GetAsync("http://localhost:8080/api/questions/redis/What%20is%20next%20to%20the%20Main%20Building%3F");
        delta = DateTime.Now - start;
        Console.WriteLine($"Redis: {delta.Milliseconds}");
    }
}