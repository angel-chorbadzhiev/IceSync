using Microsoft.EntityFrameworkCore;

using IceSync.DataAccess;
using IceSync.Models;

namespace IceSync.Services
{
    public sealed class SyncService : BackgroundService
    {
        #region Fields

        private readonly ILogger<SyncService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _minutes;
        private int _count;
        private readonly IUniversalLoaderService _universalLoaderService;

        #endregion

        #region Constructors

        public SyncService(ILogger<SyncService> logger, 
                            IUniversalLoaderService universalLoaderService,
                            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _universalLoaderService = universalLoaderService;
            _scopeFactory = serviceScopeFactory;
            _count = 0;

            _minutes = TimeSpan.FromMinutes(30);
        }

        #endregion

        #region  Overrides

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (PeriodicTimer timer = new PeriodicTimer(_minutes))
            {
                while(!stoppingToken.IsCancellationRequested &&
                        await timer.WaitForNextTickAsync(stoppingToken))
                {
                    Interlocked.Increment(ref _count);

                    try
                    {
                        await Synchronize(stoppingToken);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Failed ot execute SyncService. The error was ${ex.Message}");
                    }
                }
            }
        }

        #endregion

        #region Implementation

        private async Task Synchronize(CancellationToken stoppingToken)
        {
            List<Workflow> apiWorkflows = await _universalLoaderService.GetWorkflowsAsync();

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                using (ApplicationDbContext? dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {                    
                    await dbContext.Database.EnsureCreatedAsync(stoppingToken);

                    List<int> apiIds = new List<int>();
                    foreach (Workflow apiWorkflow in apiWorkflows)
                    {
                        apiIds.Add(apiWorkflow.WorkflowId);
                        // updates or adds workflows in the database.
                        if(dbContext.Workflows.Any(wf => wf.WorkflowId == apiWorkflow.WorkflowId))
                        {
                            // Cannot just call Update method on the Workflow DbSet
                            // because the WorkflowId is not the real id in the database.
                            // we need to update it manually.
                            Workflow? dbWorkflow = await dbContext.Workflows.FirstOrDefaultAsync(wf => wf.WorkflowId == apiWorkflow.WorkflowId);
                            if (dbWorkflow == null)
                                continue;
                            
                            dbWorkflow.IsActive = apiWorkflow.IsActive;
                            dbWorkflow.IsRunning = apiWorkflow.IsRunning;
                            dbWorkflow.WorkflowName = apiWorkflow.WorkflowName;
                            dbWorkflow.MultiExecBehavior = apiWorkflow.MultiExecBehavior;

                            dbContext.Workflows.Update(dbWorkflow);
                        }
                        else
                        {
                            dbContext.Workflows.Add(apiWorkflow);
                        }
                    }
                    
                    // delete workflows in the database whcih are not in the api call result.
                    await dbContext.Workflows.Where(wf => !apiIds.Contains(wf.WorkflowId)).ExecuteDeleteAsync();

                    await dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Workflows synchronization complete.");
                }
            }
        }

        #endregion
    }
}