using AlinSpace.Development;
using AlinSpace.Development.Workflow;
using DryIoc;
using System.Diagnostics;

namespace AlinSpace.Development.Console.Commands.Build
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
            await workflowService.BuildAsync(incrementBuild: options.Increment);
        }
    }
}