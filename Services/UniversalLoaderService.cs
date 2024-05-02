using System.Text.Json;
using Microsoft.Extensions.Options;

using IceSync.DataAccess;
using IceSync.Helpers;
using IceSync.Models;


namespace IceSync.Services
{
    public class UniversalLoaderService : IUniversalLoaderService
    {
        #region Fields
        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;

        #endregion

        #region Constructors
        public UniversalLoaderService(ILogger<UniversalLoaderService> logger, 
                                      HttpClient httpClient) 
        {
            _logger = logger;
            _httpClient = httpClient;            
        }      

        #endregion

        #region Endpoints

        public async Task<List<Workflow>> GetWorkflowsAsync()
        {
            Uri uri = new Uri(_httpClient.BaseAddress, "Workflows/");
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            string responseContent = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<WorkflowDto>? workflowDtos = JsonSerializer.Deserialize<List<WorkflowDto>>(responseContent, options);

            if(workflowDtos == null)
            {
                workflowDtos = new List<WorkflowDto>();   
            }

            // Map the received workflows from the api to our Workflow model type.

            // Here we can use AutoMapper library but since we have only one model and to keep the project 
            // clean from to many references I am doing it manually via extension method.
            List<Workflow> workflows = new List<Workflow>();
            foreach(WorkflowDto workflowDto in workflowDtos)
            {
                workflows.Add(workflowDto.MapToModel());
            }

            return workflows;
        }

        public async Task<string> RunAsync(int id)
        {
            string message = "The command did not complete.";

            try
            {
                Uri uri = new Uri(_httpClient.BaseAddress, $"workflows/{id.ToString()}/run");
                HttpResponseMessage response = await _httpClient.PostAsync(uri, null);               
                
                if(response.IsSuccessStatusCode)
                {
                    message = "The command complete successfully.";
                }

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return message;
            }
        }

        #endregion       
    }
}
