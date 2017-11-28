using Autofac;

namespace Budget.Setup
{
    public class BudgetContainerSetup : Module
    {
        private readonly BudgetDbSetup _dbSetup;

        public BudgetContainerSetup(BudgetDbSetup dbSetup)
        {
            _dbSetup = dbSetup;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _dbSetup.CreateDbContext())
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Services"))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
