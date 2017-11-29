using Budget.Core.Model;

namespace Budget.Specs.Helpers
{
    // 11-1. Add BudgetClassData helper class
    //---------------------------------------
    public class BudgetClassData
    {
        public decimal BaseTotal { get; set; }

        public string BudgetClass { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public TransactionType TransactionType { get; set; } = TransactionType.Income;
    }
}
