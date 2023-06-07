using AbstractModel.EFCore;
using CashFlow.Context;
using CashFlow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.CRUD
{
    public class CRUDAccount : AbstractContext<Account>
    {
        internal CRUDAccount(CashFlowContext context) : base(context) { }
    }
}
