using AlinSpace.Development.Workflow;
using DryIoc;

namespace AlinSpace.Development.Console.Commands.Configuration
{
    public class Command
    {
        private readonly IWorkflowService workflowService;

        public Command()
        {
            workflowService = RootContainer.Instance.Resolve<IWorkflowService>();
        }

        public async Task ExecuteAsync(Options options)
        {
            await workflowService.ShowConfigurationAsync();
        }
    }
}