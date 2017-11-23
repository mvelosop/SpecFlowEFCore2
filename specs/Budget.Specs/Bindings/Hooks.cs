using Autofac;
using TechTalk.SpecFlow;

namespace Budget.Specs.Bindings
{
    [Binding]
    public sealed class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private static IContainer _container;
        private readonly ScenarioContext _scenarioContext;

        public Hooks(
            ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Startup startup = Startup.Create();

            _container = startup.Container;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _scenarioContext.Set(_container, Startup.ContainerKey);
        }

        private ILifetimeScope GetLifetimeScope()
        {
            return _scenarioContext.Get<IContainer>(Startup.ContainerKey).BeginLifetimeScope();
        }
    }
}
