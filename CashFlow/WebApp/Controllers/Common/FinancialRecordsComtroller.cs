using CashFlow.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Common
{
    public class FinancialRecordsComtroller : ControllerBase
    {
        public FinancialRecordsComtroller(CashFlowContext cashflowContext) : base(cashflowContext) { }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                return LogOff();
            }
            else
            {
                return View(PathViews.FinancialRecords);
            }
        }
    }
}
