using Budget.Core.Model;
using Budget.Data.Services;
using Domion.Extensions;
using Domion.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.App.Services
{
    // 3-1. Add BudgetClassServices
    //-----------------------------

    public class BudgetClassServices
    {
        private readonly Lazy<BudgetClassRepository> _lazyBudgetClassRepo;

        public BudgetClassServices(
            Lazy<BudgetClassRepository> lazyBudgetClassRepo)
        {
            _lazyBudgetClassRepo = lazyBudgetClassRepo;
        }

        private BudgetClassRepository BudgetClassRepo => _lazyBudgetClassRepo.Value;

        public async Task<List<ValidationResult>> AddBudgetClassAsync(BudgetClass entity)
        {
            // 3-4. Save to repo
            //------------------

            //List<ValidationResult> errors = await BudgetClassRepo.TryInsertAsync(entity);

            //if (errors.Any()) return errors;

            //await BudgetClassRepo.SaveChangesAsync();

            return Errors.NoError;
        }

        public IQueryable<BudgetClass> QueryBudgetClasses(Expression<Func<BudgetClass, bool>> where = null)
        {
            // 3-5. Get data from repo
            //------------------------

            //return BudgetClassRepo.Query(where);

            return Enumerable.Empty<BudgetClass>().AsAsyncQueryable();
        }
    }

}
