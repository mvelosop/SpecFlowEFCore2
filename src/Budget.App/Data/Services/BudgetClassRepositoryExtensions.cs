﻿using Budget.Core.Model;
using Domion.Data.Base;
using Domion.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Data.Services
{
    public static class BudgetClassRepositoryExtensions
    {
        public static async Task<BudgetClass> FindByIdAsync(this IEntityQuery<BudgetClass> repository, int id)
        {
            return await repository.FirstOrDefaultAsync(bc => bc.Id == id);
        }

        public static async Task<BudgetClass> FindByNameAsync(this IEntityQuery<BudgetClass> repository, string name)
        {
            return await repository.FirstOrDefaultAsync(bc => bc.Name == name.Trim());
        }

        public static async Task<BudgetClass> FindDuplicateByNameAsync(this IEntityQuery<BudgetClass> repository, BudgetClass entity)
        {
            IQueryable<BudgetClass> query = repository.Query(bc => bc.Name == entity.Name.Trim());

            if (entity.Id != 0)
            {
                query = query.Where(bc => bc.Id != entity.Id);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
