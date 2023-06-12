using CashFlow.Context;
using Microsoft.AspNetCore.Mvc;
using Proxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Tests
{
    public  class ControllerBase : Controller
    {
        private CashFlowContext CashFlowContext { get; set; }

        public IProxyServices IProxyServices => new ProxyServices(CashFlowContext);

        public ControllerBase() { }

        public ControllerBase(CashFlowContext cashflowContext)
        {
            CashFlowContext = cashflowContext;
        }
    }
}
