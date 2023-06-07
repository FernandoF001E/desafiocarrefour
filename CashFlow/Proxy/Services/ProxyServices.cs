using CashFlow.Context;
using CashFlow.Services;
using CryptoSecurity.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Services
{
    public class ProxyServices : IProxyServices
    {
        private readonly CashFlowContext? CashFlowContext;

        public ICashFlowServices CashFlow => new CashFlowActionsServices(CashFlowContext);

        public ICryptoServices CryptoServices => new CryptoServices();

        public ProxyServices() { }

        public ProxyServices(CashFlowContext cashflowContext)
        {
            CashFlowContext = cashflowContext;
        }
    }
}
