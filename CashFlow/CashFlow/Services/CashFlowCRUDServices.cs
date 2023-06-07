using CashFlow.Context;
using CashFlow.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Services
{
    public class CashFlowCRUDServices
    {
        private readonly CashFlowContext _context = null;

        public CashFlowCRUDServices(CashFlowContext context)
        {
            _context = context;
        }

        public CRUDAccount Account => new(_context);
        public CRUDFinancialRecords FinancialRecords => new(_context);
    }
}
