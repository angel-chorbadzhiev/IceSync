using IceSync.Models;

namespace IceSync.Services
{
    public interface IUniversalLoaderService
    {
        /// <summary>
        /// Gets all <see cref="Workflow"/>s asynchronously.
        /// </summary>
        /// <returns>List with Workflows.</returns>
        public Task<List<Workflow>> GetWorkflowsAsync();

        /// <summary>
        /// Run a specified workflow by its Id.
        /// </summary>
        /// <param name="id">The workflow Id.</param>
        /// <returns>Message whether the call was successful or not.</returns>
        public Task<string> RunAsync(int id);
    }
}
