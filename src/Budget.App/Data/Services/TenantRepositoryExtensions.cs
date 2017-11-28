using Budget.Core.Model;
using Domion.Data.Base;
using Domion.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Data.Services
{
    // 6-5. Add TenantRepositoryExtensions
    //------------------------------------

    public static class TenantRepositoryExtensions
    {
        public static async Task<Tenant> FindByIdAsync(this IEntityQuery<Tenant> repository, int id)
        {
            return await repository.FirstOrDefaultAsync(bc => bc.Id == id);
        }

        public static async Task<Tenant> FindByNameAsync(this IEntityQuery<Tenant> repository, string name)
        {
            return await repository.FirstOrDefaultAsync(bc => bc.Name == name.Trim());
        }

        public static async Task<Tenant> FindDuplicateByNameAsync(this IEntityQuery<Tenant> repository, Tenant entity)
        {
            IQueryable<Tenant> query = repository.Query(bc => bc.Name == entity.Name.Trim());

            if (entity.Id != 0)
            {
                query = query.Where(bc => bc.Id != entity.Id);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
