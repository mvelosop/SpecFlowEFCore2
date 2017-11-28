using Autofac;
using Budget.App.Services;
using Budget.Core.Model;
using Budget.Data.Services;
using Domion.Testing.Assertions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Budget.Specs.Bindings
{
    // 6-7. Add TenantsStepDefinitions
    //--------------------------------

    [Binding]
    public sealed class TenantsStepDefinitions
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        public TenantsStepDefinitions(
            ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"tenant ""(.*)"" does not exist")]
        public async Task GivenTenantDoesNotExistAsync(string name)
        {
            await EnsureTenantDoesNotExist(name);
        }

        [Given(@"these tenants don't exist:")]
        public async Task GivenTheseTenantsDonTExist(Table table)
        {
            var dataSet = table.CreateSet<Tenant>();

            foreach (Tenant t in dataSet)
            {
                await EnsureTenantDoesNotExist(t.Name);
            }
        }

        [Then(@"I can't add another tenant ""(.*)""")]
        public async Task ThenICanTAddAnotherTenant(string name)
        {
            var services = Resolve<TenantServices>();

            var tenant = new Tenant
            {
                Name = name,
            };

            var errors = await services.AddTenantAsync(tenant);

            errors.Should().ContainErrorMessage(TenantRepository.DuplicateByNameError);
        }

        [Then(@"the following tenants exist:")]
        public async Task ThenTheFollowingTenantsExist(Table table)
        {
            var services = Resolve<TenantServices>();

            List<Tenant> result = await services.QueryTenants().ToListAsync();

            var expectedSet = table.CreateSet<Tenant>()
                .Select(t => t.Name);

            result.Select(t => t.Name).Should().Contain(expectedSet);
        }

        [When(@"I add tenant ""(.*)""")]
        public async Task WhenIAddTenant(string name)
        {
            await AddTenant(name);
        }

        [When(@"I add tenants:")]
        public async Task WhenIAddTenants(Table table)
        {
            var dataSet = table.CreateSet<Tenant>();

            var services = Resolve<TenantServices>();

            foreach (Tenant bc in dataSet)
            {
                var errors = await services.AddTenantAsync(bc);

                errors.Should().BeEmpty();
            }
        }

        private async Task AddTenant(string name)
        {
            var services = Resolve<TenantServices>();

            var tenant = new Tenant
            {
                Name = name,
            };

            var errors = await services.AddTenantAsync(tenant);

            errors.Should().BeEmpty();
        }

        private async Task EnsureTenantDoesNotExist(string name)
        {
            var services = Resolve<TenantServices>();

            var tenant = await services.FindTenantByNameAsync(name);

            if (tenant == null) return;

            var errors = await services.RemoveTenantAsync(tenant);

            errors.Should().BeEmpty();
        }

        private IContainer GetContainer()
        {
            return _scenarioContext.Get<IContainer>(Startup.ContainerKey);
        }

        private ILifetimeScope GetScope()
        {
            return GetContainer().BeginLifetimeScope();
        }

        private T Resolve<T>() where T : class
        {
            return _scenarioContext.Get<ILifetimeScope>(Startup.ScopeKey)?.Resolve<T>();
        }
    }
}
