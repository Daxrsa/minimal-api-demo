using minimal_api_demo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped(hc => new HttpClient{BaseAddress = new Uri("https://my-json-server.typicode.com/")});

//4.HttpRequestMessage Object
builder.Services.AddHttpClient();

//1.Explicit http client
builder.Services.AddHttpClient("my-json-server", client => 
{
    client.BaseAddress = new Uri("https://my-json-server.typicode.com/");
}); //this approach makes it impossible to consume multiple domains

//3.Type Client
builder.Services.AddHttpClient<MyJsonServerHttpClient>(client => 
{
    client.BaseAddress = new Uri("https://my-json-server.typicode.com/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//1.Explicit http client
app.MapGet("explicit-http-client", async (HttpClient httpClient) => 
{
    return await httpClient.GetFromJsonAsync<List<Post>>("typicode/demo/posts");
});

//2.Named Client
app.MapGet("/named-client", async(IHttpClientFactory factory) => 
{
    var httpClient = factory.CreateClient("my-json-server");
    return await httpClient.GetFromJsonAsync<List<Post>>("typicode/demo/posts");
});

//3.Typed Client
app.MapGet("/typed-client", async(MyJsonServerHttpClient client) => 
{
    return await client.AllPosts();
});

//4.HttpRequestMessage Object
app.MapGet("http-request-object", async(IHttpClientFactory factory) => 
{
    var request = new HttpRequestMessage(HttpMethod.Get, "https://my-json-server.typicode.com/typicode/demo/posts");
    var httpClient = factory.CreateClient();
    var response = await httpClient.SendAsync(request); //api call will be invoked
    return await response.Content.ReadFromJsonAsync<List<Post>>();
});

app.Run();
