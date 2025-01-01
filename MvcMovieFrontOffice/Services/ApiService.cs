using Newtonsoft.Json;

namespace MvcMovieFrontOffice.Services;

public class ApiService(HttpClient httpClient)
{
    public async Task<T?> GetDataFromApiAsync<T>(string apiUrl)
    {
        var response = await httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode) return default(T);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);

    }
    
    public async Task<bool> PostDataToApiAsync<T>(string apiUrl, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(apiUrl, content);
        
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> PutDataToApiAsync<T>(string apiUrl, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        Console.WriteLine($"Payload envoyé : {json}");

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync(apiUrl, content);
        
        return response.IsSuccessStatusCode;
    }
}