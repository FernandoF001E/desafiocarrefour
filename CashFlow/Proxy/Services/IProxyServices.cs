using CashFlow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Services
{
    public interface IProxyServices
    {
        ICashFlowServices CashFlow { get; }
    }
}
