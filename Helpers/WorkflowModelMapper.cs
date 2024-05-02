using IceSync.Models;

namespace IceSync.Helpers
{
    /// <summary>
    /// Exstension methods for WorkflowDto model class.
    /// </summary>
    public static class WorkflowModelMapper
    {
        /// <summary>
        /// Maps WorkwlowDto model to Workflow model.
        /// </summary>
        /// <param name="workflowDto"></param>
        /// <returns></returns>
        public static Workflow MapToModel(this WorkflowDto workflowDto)
        {
            return new Workflow()
            {
                WorkflowId = workflowDto.Id,
                WorkflowName = workflowDto.Name,
                IsActive = workflowDto.IsActive,
                MultiExecBehavior = workflowDto.MultiExecBehavior,
                // TODO: Not sure from where to obtain the IsRunning value from.
                IsRunning = true
            };            
        }        
    }
}