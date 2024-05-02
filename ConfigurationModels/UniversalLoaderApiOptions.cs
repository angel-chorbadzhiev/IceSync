namespace IceSync
{
    /// <summary>
    /// Obtains the Universal Loader API credentials from the configuration file.
    /// </summary> 
    public class UniversalLoaderApiOptoins
    {
        public const string Credentials = "UniversalLoaderApiCredentials";

        #region Properties

        public string ApiCompanyId { get; set; } = string.Empty;
        public string ApiUserId { get; set; } = string.Empty;
        public string ApiUserSecret { get; set; } = string.Empty;

        #endregion
    }
}