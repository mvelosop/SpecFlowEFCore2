using Budget.Core.Model;

namespace Budget.App
{
    // 7-1. Add SessionContext
    //------------------------

    public class SessionContext
    {
        public SessionContext(
            Tenant tenant)
        {
            CurrentTenant = tenant;
        }

        public Tenant CurrentTenant { get; }
    }
}
