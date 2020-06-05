using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

public class Request
{
    private HttpClient _client;

    public Request(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<string> MakeGetRequest(string resource)
    {
        try
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(_client.BaseAddress, resource),
                Method = HttpMethod.Get,
            };
            var response = await _client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // you need to maybe re-authenticate here
                return default(string);
            }
            else
            {
                return default(string);
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}