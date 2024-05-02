using Microsoft.Extensions.Options;

namespace IceSync.Services
{
    /// <summary>
    /// Authentication service which call Authenticate endpoint and provides JWT token.
    /// </summary>    
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly UniversalLoaderApiOptoins _options;

        public AuthService(IHttpClientFactory httpClientFactory,IOptions<UniversalLoaderApiOptoins> options)
        {
            _httpClient = httpClientFactory.CreateClient("AuthClient");
            _options = options.Value;
        }

        public async Task<string> GetJwtToken()
        { 
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/authenticate", _options);
            string token = await httpResponse.Content.ReadAsStringAsync();
            
            return token;
        }        
    }
}