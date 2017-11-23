using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Budget.Specs
{
    public class Startup
    {
        public const string ContainerKey = "Container";
        public const string ScopeKey = "Scope";

        private static object _lock = new object();
        private static Startup _startup;

        public Startup()
        {
            Options = ConfigureOptions();

            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);
        }

        public IContainer Container { get; private set; }

        public IConfigurationRoot Options { get; }

        public static Startup Create()
        {
            lock (_lock)
            {
                if (_startup == null)
                {
                    _startup = new Startup();
                }
            }

            return _startup;
        }

        private void ConfigureContainer(ContainerBuilder builder)
        {
            // TODO: Configure Autofac container
        }

        private IConfigurationRoot ConfigureOptions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            return configuration;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            ConfigureContainer(builder);

            Container = builder.Build();
        }
    }
}
