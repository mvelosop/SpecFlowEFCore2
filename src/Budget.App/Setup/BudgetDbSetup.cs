using Budget.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace Budget.Setup
{
    public class BudgetDbSetup
    {
        private readonly string _connectionString;

        private readonly object _lock = new object();

        private DbContextOptions<BudgetDbContext> _options;

        public BudgetDbSetup(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ConfigureDatabase(bool migrateDatabase = false)
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            _options = optionBuilder.Options;

            if (!migrateDatabase) return;

            // This lock avoids conflicts on DB creation, specially during parallel integration tests
            lock (_lock)
            {
                using (var dbContext = CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

        public BudgetDbContext CreateDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {GetType().Name}.{nameof(ConfigureDatabase)} first!");

            return new BudgetDbContext(_options);
        }
    }
}
