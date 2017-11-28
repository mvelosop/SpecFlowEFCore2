using Budget.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Scripts.Cli
{
    // 1-1. Add DbContext factory
    //---------------------------

    public class BudgetDbContextDesignTimeFactory : IDesignTimeDbContextFactory<BudgetDbContext>
    {
        public BudgetDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BudgetDbContext>();

            builder.UseSqlServer("x");

            return new BudgetDbContext(builder.Options);
        }
    }
}
