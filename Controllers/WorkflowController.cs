using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using IceSync.Models;
using IceSync.Services;

namespace IceSync.Controllers
{
    /// <summary>
    /// Controller for the Workflow views
    /// </summary>
    public class WorkflowController : Controller
    {
        #region  Fields
        
        private readonly ILogger<WorkflowController> _logger;

        private readonly IUniversalLoaderService _loaderService;

        #endregion

        #region Constructors

        public WorkflowController(ILogger<WorkflowController> logger, IUniversalLoaderService loaderService)
        {
            _logger = logger;
            _loaderService = loaderService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Gets all workflows from the API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Workflow> workflows = await _loaderService.GetWorkflowsAsync();

            return View(workflows);
        }

        /// <summary>
        /// Posts the Run endpoint on the API for specified Id
        /// </summary>
        /// <param name="id">The Id of the Workflow</param>
        /// <returns></returns> 
        [HttpPost]
        public async Task<IActionResult> Run(int id)
        {
            string result = await _loaderService.RunAsync(id);
            
            return Ok(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });            
        }

        #endregion
    }
}