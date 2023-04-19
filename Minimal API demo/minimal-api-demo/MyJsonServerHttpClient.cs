namespace minimal_api_demo
{
    public class MyJsonServerHttpClient
    {
        public readonly HttpClient _httpClient;
        public MyJsonServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> AllPosts()
        {
            return await _httpClient.GetFromJsonAsync<List<Post>>("typicode/demo/posts");
        }
    }
}