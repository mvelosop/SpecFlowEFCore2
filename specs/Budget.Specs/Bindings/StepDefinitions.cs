﻿using Autofac;
using Budget.App;
using Budget.App.Services;
using Budget.Core.Model;
using Budget.Data.Services;
using Budget.Specs.Helpers;
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
    [Binding]
    public sealed class StepDefinitions
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        // 10-1. Inject FeatureContext
        //----------------------------
        private readonly FeatureContext _featureContext;

        private readonly ScenarioContext _scenarioContext;

        public StepDefinitions(
            FeatureContext featureContext,
            ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        // 7-3. Add budget classes to tenant step
        //---------------------------------------
        [Given(@"I have the following budget class for ""(.*)"":")]
        [Then(@"I can also have the following budget class for ""(.*)"":")]
        public async Task GivenIHaveTheFollowingBudgetClassFor(string name, Table table)
        {
            var tenantServices = Resolve<TenantServices>();

            var currentTenant = await tenantServices.FindTenantByNameAsync(name);

            var sessionContext = new SessionContext(currentTenant);

            using (var scope = GetContainer().BeginLifetimeScope(builder => builder.Register(c => sessionContext)))
            {
                var dataSet = table.CreateSet<BudgetClass>();

                var budgetClassServices = scope.Resolve<BudgetClassServices>();

                foreach (BudgetClass bc in dataSet)
                {
                    var errors = await budgetClassServices.AddBudgetClassAsync(bc);

                    errors.Should().BeEmpty();
                }
            }
        }

        // 9-2. Create Scenario tenant context
        //------------------------------------
        [Given(@"I'm working in a new scenario tenant context")]
        public async Task GivenImWorkingInANewScenarioTenantContext()
        {
            // Get scenario name
            var scenarioName = _scenarioContext.ScenarioInfo.Title;

            var sessionContext = await GetSessionContext(scenarioName);

            _scenarioContext.Set(sessionContext, nameof(SessionContext));
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
        // 11-5. Map "Given" Clause
        //-----------------------------
        [When(@"I add budget classes:")]
        [Given(@"I have added these budget classes:")]
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

        // 12-2. Add delete step
        //----------------------
        [When(@"I delete budget class ""(.*)""")]
        public async Task WhenIDeleteBudgetClass(string name)
        {
            var services = Resolve<BudgetClassServices>();

            var entity = await services.FindBudgetClassByNameAsync(name);

            entity.Should().NotBeNull($@"because BudgetClass ""{name}"" MUST exist!");

            var errors = await services.RemoveBudgetClassAsync(entity);

            errors.Should().BeEmpty();
        }

        // 11-6. Add update step
        //----------------------
        [When(@"I update the budget classes to this:")]
        public async Task WhenIUpdateTheBudgetClassesToThis(Table table)
        {
            var dataSet = table.CreateSet<BudgetClassData>();

            var services = Resolve<BudgetClassServices>();
            var mapper = Resolve<BudgetClassMapper>();

            foreach (BudgetClassData bcd in dataSet)
            {
                var entity = await services.FindBudgetClassByNameAsync(bcd.BudgetClass);

                entity.Should().NotBeNull($@"because BudgetClass ""{bcd.BudgetClass}"" MUST exist!");

                entity = mapper.UpdateEntity(bcd, entity);

                var errors = await services.UpdateBudgetClassAsync(entity);

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

        // 9-3. Create tenant context for session
        //---------------------------------------
        private async Task<SessionContext> GetSessionContext(string scenarioName)
        {
            var services = Resolve<TenantServices>();

            var tenant = await services.FindTenantByNameAsync(scenarioName);

            // 10-2. Reset tenant data just once per scenario
            //-----------------------------------------------

            if (!_featureContext.ContainsKey(scenarioName))
            {
                _featureContext.Set(true, scenarioName);

                if (tenant != null)
                {
                    var dbContext = Resolve<BudgetDbContext>();

                    dbContext.RemoveRange(await dbContext.BudgetClasses.Where(bc => bc.Tenant_Id == tenant.Id).ToListAsync());
                    await dbContext.SaveChangesAsync();

                    await services.RemoveTenantAsync(tenant);
                }

                tenant = new Tenant { Name = scenarioName };

                var errors = await services.AddTenantAsync(tenant);

                errors.Should().BeEmpty();
            }

            return new SessionContext(tenant);
        }

        // 4-4. Resolve dependency from current scope
        //-------------------------------------------
        private T Resolve<T>() where T : class
        {
            return _scenarioContext.Get<ILifetimeScope>(Startup.ScopeKey)?.Resolve<T>();
        }
    }
}
