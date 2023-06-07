using AbstractModel.EFCore;
using CashFlow.Context;
using CashFlow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.CRUD
{
    public class CRUDFinancialRecords : AbstractContext<FinancialRecords>
    {
        internal CRUDFinancialRecords(CashFlowContext context) : base(context) { }
    }
}
