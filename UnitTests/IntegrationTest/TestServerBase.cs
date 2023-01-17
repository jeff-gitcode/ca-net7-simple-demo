using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

public class TestFixture<TStartup> : IDisposable where TStartup : class
{
    private readonly TestServer _testServer;

    public TestFixture()
    {
        var webHostBuilder = new WebHostBuilder().UseStartup<TStartup>();
        _testServer = new TestServer(webHostBuilder);

        httpClient = _testServer.CreateClient();
        httpClient.BaseAddress = new Uri("http://localhost:5106");
    }

    public HttpClient httpClient { get; }

    public void Dispose()
    {
        httpClient.Dispose();
        _testServer.Dispose();
    }
}