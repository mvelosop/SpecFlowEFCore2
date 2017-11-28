using Autofac;
using Budget.App.Services;
using Budget.Core.Model;
using Budget.Data.Services;
using Domion.Testing.Assertions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budget.App;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Budget.Specs.Bindings
{
    [Binding]
    public sealed class StepDefinitions
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        public StepDefinitions(
            ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        // 4-1. Clear data step
        //---------------------

        [Given(@"there are no BudgetClasses")]
        public async Task GivenThereAreNoBudgetClasses()
        {
            // 4-5. Refactor dependency resolution
            //------------------------------------

            var dbContext = Resolve<BudgetDbContext>();

            dbContext.RemoveRange(await dbContext.BudgetClasses.ToListAsync());
            await dbContext.SaveChangesAsync();
        }

        // 7-3. Add budget classes to tenant step
        //---------------------------------------

        //[Given(@"I have the following budget class for ""(.*)"":")]
        //[Then(@"I can also have the following budget class for ""(.*)"":")]
        //public async Task GivenIHaveTheFollowingBudgetClassFor(string name, Table table)
        //{
        //    var tenantServices = Resolve<TenantServices>();

        //    var currentTenant = await tenantServices.FindTenantByNameAsync(name);

        //    var sessionContext = new SessionContext(currentTenant);

        //    using (var scope = GetContainer().BeginLifetimeScope(builder => builder.Register(c => sessionContext)))
        //    {
        //        var dataSet = table.CreateSet<BudgetClass>();

        //        var budgetClassServices = scope.Resolve<BudgetClassServices>();

        //        foreach (BudgetClass bc in dataSet)
        //        {
        //            var errors = await budgetClassServices.AddBudgetClassAsync(bc);

        //            errors.Should().BeEmpty();
        //        }
        //    }
        //}

        // 5-2. Verify duplicate name step
        //--------------------------------

        [Then(@"I can't add another class ""(.*)""")]
        public async Task ThenICanTAddAnotherClass(string name)
        {
            var services = Resolve<BudgetClassServices>();

            var budgetClass = new BudgetClass
            {
                Name = name,
            };

            var errors = await services.AddBudgetClassAsync(budgetClass);

            errors.Should().ContainErrorMessage(BudgetClassRepository.DuplicateByNameError);
        }

        // 3-2. Get budget classes step
        //-----------------------------

        [Then(@"I get the following budget classes")]
        public async Task ThenIGetTheFollowingBudgetClasses(Table table)
        {
            // 4-5. Refactor dependency resolution
            //------------------------------------

            var services = Resolve<BudgetClassServices>();

            List<BudgetClass> result = await services.QueryBudgetClasses().ToListAsync();

            table.CompareToSet(result);
        }

        // 5-1. Add budget class step
        //---------------------------

        [When(@"I add budget class ""(.*)""")]
        public async Task WhenIAddBudgetClass(string name)
        {
            var services = Resolve<BudgetClassServices>();

            var budgetClass = new BudgetClass
            {
                Name = name,
            };

            var errors = await services.AddBudgetClassAsync(budgetClass);

            errors.Should().BeEmpty();
        }

        // 3-3. Add budget classes step
        //-----------------------------

        [When(@"I add budget classes:")]
        public async Task WhenIAddBudgetClasses(Table table)
        {
            var dataSet = table.CreateSet<BudgetClass>();

            // 4-5. Refactor dependency resolution
            //------------------------------------

            var services = Resolve<BudgetClassServices>();

            foreach (BudgetClass bc in dataSet)
            {
                var errors = await services.AddBudgetClassAsync(bc);

                errors.Should().BeEmpty();
            }

        }

        private IContainer GetContainer()
        {
            return _scenarioContext.Get<IContainer>(Startup.ContainerKey);
        }

        private ILifetimeScope GetScope()
        {
            return GetContainer().BeginLifetimeScope();
        }

        // 4-4. Resolve dependency from current scope
        //-------------------------------------------

        private T Resolve<T>() where T : class
        {
            return _scenarioContext.Get<ILifetimeScope>(Startup.ScopeKey)?.Resolve<T>();
        }
    }
}
