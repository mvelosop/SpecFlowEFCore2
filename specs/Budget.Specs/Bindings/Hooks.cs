using Autofac;
using Budget.App;
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

        // 4-3. Dispose scope from scenario context
        //-----------------------------------------

        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TryGetValue(Startup.ScopeKey, out ILifetimeScope scope))
            {
                scope?.Dispose();

                _scenarioContext.Remove(Startup.ScopeKey);
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _scenarioContext.Set(_container, Startup.ContainerKey);
        }

        // 4-2. Store scope in scenario context
        //-------------------------------------

        [BeforeStep]
        public void BeforeStep()
        {
            ILifetimeScope scope = GetLifetimeScope();

            _scenarioContext.Set(scope, Startup.ScopeKey);
        }

        private ILifetimeScope GetLifetimeScope()
        {
            // 9-1. Register session context in scope
            //---------------------------------------

            if (_scenarioContext.TryGetValue(nameof(SessionContext), out SessionContext sessionContext))
            {
                return _scenarioContext.Get<IContainer>(Startup.ContainerKey).BeginLifetimeScope(
                    builder => builder.Register(c => sessionContext));
            }

            return _scenarioContext.Get<IContainer>(Startup.ContainerKey).BeginLifetimeScope();
        }
    }
}
