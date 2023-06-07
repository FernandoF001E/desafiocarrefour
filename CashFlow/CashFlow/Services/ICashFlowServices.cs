using CashFlow.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Services
{
    public interface ICashFlowServices
    {
        CRUDAccount Account { get; }
        CRUDFinancialRecords FinancialRecords { get; }
    }
}
