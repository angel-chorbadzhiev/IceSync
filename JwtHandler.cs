using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IceSync.Services;
using Microsoft.Extensions.Options;

namespace IceSync
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;
        private readonly IStorageService _storageService;
        private readonly ILogger<JwtHandler> _logger;

        
        public JwtHandler(ILogger<JwtHandler> logger, 
                            IAuthService authService,
                            IStorageService storageService)
        {
            _logger = logger;
            _authService = authService;
            _storageService = storageService;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if there is a token stored in the storage service.
            if(string.IsNullOrWhiteSpace(_storageService.JwtToken))
            {
                string token = await _authService.GetJwtToken();
                _storageService.JwtToken = token;
            }

            // Add token to the request header.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _storageService.JwtToken);

            // Make the request.
            HttpResponseMessage httpResponse = await base.SendAsync(request, cancellationToken);

            // If the response was unsuccessful with error 401 create new token.
            // Add the new token to the requset header and make a new requset.
            if(httpResponse.IsSuccessStatusCode)
                return httpResponse;

            if(httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                string token = await _authService.GetJwtToken();
                _storageService.JwtToken = token;

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _storageService.JwtToken);    
            }

            return await base.SendAsync(request, cancellationToken);
        }        
    }
}