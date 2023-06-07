using CashFlow.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Services
{
    public class CashFlowActionsServices : CashFlowCRUDServices, ICashFlowServices
    {
        private readonly CashFlowContext Context = null;

        public CashFlowActionsServices(CashFlowContext context) : base(context)
        {
            Context = context;
        }
    }
}
