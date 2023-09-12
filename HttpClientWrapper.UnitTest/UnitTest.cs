using HttpClientWrapper;
using HttpClientWrapper.Test;

public class UnitTest
{
    const string url = "https://dummyjson.com/products";
    private IHttpClientActor httpClient;

    public UnitTest()
    {
        httpClient = new HttpClientActor(url);
    }

    [Fact]
    public async Task GetAll()
    {
        httpClient.SetBaseUri(url);
        ProductList data = await httpClient.GetAsync<ProductList>(string.Empty);
        Assert.NotNull(data);
    }
}