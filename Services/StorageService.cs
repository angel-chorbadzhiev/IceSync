namespace IceSync.Services
{
    /// <summary>
    /// Singleton service which stores the JWT token.
    /// </summary>
    public class StorageService : IStorageService
    {
        public string JwtToken { get; set; } = string.Empty;        
    }
}