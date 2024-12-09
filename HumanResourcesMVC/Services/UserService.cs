using System.Text.Json;

namespace HumanResourcesMVC.Services
{
	public class UserService
	{
		private readonly HttpClient _httpClient;

		public UserService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<string> GetPasswordSetTokenAsync(string email)
		{
			var response = await _httpClient.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Account/GeneratePasswordSetToken", email);
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase // CamelCase özelliğini kullan
			});

			return result["token"];
		}
	}
}
