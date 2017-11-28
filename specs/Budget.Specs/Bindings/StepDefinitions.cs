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
            using (var scope = GetScope())
            {
                var dbContext = scope.Resolve<BudgetDbContext>();

                dbContext.RemoveRange(await dbContext.BudgetClasses.ToListAsync());
                await dbContext.SaveChangesAsync();
            }

            // 4-5. Refactor dependency resolution
            //------------------------------------

            //var dbContext = Resolve<BudgetDbContext>();

            //dbContext.RemoveRange(await dbContext.BudgetClasses.ToListAsync());
            //await dbContext.SaveChangesAsync();
        }

        // 3-2. Get budget classes step
        //-----------------------------

        [Then(@"I get the following budget classes")]
        public async Task ThenIGetTheFollowingBudgetClasses(Table table)
        {
            using (var scope = GetScope())
            {
                var services = scope.Resolve<BudgetClassServices>();

                List<BudgetClass> result = await services.QueryBudgetClasses().ToListAsync();

                table.CompareToSet(result);
            }

            // 4-5. Refactor dependency resolution
            //------------------------------------

            //var services = Resolve<BudgetClassServices>();

            //List<BudgetClass> result = await services.QueryBudgetClasses().ToListAsync();

            //table.CompareToSet(result);
        }

        // 3-3. Add budget classes step
        //-----------------------------

        [When(@"I add budget classes:")]
        public async Task WhenIAddBudgetClasses(Table table)
        {
            var dataSet = table.CreateSet<BudgetClass>();

            using (var scope = GetScope())
            {
                var services = scope.Resolve<BudgetClassServices>();

                foreach (BudgetClass bc in dataSet)
                {
                    var errors = await services.AddBudgetClassAsync(bc);

                    errors.Should().BeEmpty();
                }
            }

            // 4-5. Refactor dependency resolution
            //------------------------------------

            //var services = Resolve<BudgetClassServices>();

            //foreach (BudgetClass bc in dataSet)
            //{
            //    var errors = await services.AddBudgetClassAsync(bc);

            //    errors.Should().BeEmpty();
            //}

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

        //private T Resolve<T>() where T : class
        //{
        //    return _scenarioContext.Get<ILifetimeScope>(Startup.ScopeKey)?.Resolve<T>();
        //}
    }
}
