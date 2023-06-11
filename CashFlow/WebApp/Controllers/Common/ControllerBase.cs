using CashFlow.Context;
using CashFlow.Data;
using CryptoSecurity.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Proxy.Services;
using System;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class ControllerBase : Controller
    {
        public const string SessionUserKey = "_Key";

        private int _limit = 0;
        private int _index = 0;
        public int _countData = 0;
        public int _totalPages = 0;

        public int PagingLimit
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.Query["limit"]))
                    {
                        _limit = Convert.ToInt32(Request.Query["limit"]);
                    }
                }
                catch
                {
                    //--> Ignore
                }
                return _limit;
            }
            set => _limit = value;
        }

        public int PagingIndex
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.Query["index"]))
                    {
                        _index = Convert.ToInt32(Request.Query["index"]);
                    }
                }
                catch
                {
                    //--> Ignore
                }
                return _index;
            }
            set => _index = value;
        }

        public int PagingCount
        {
            get => _countData;
            set => _countData = value;
        }

        public int PagingTotal
        {
            get => _totalPages;
            set => _totalPages = value;
        }

        public ApplicationConfig AppConfigOptions;

        public Account UserSession { get; set; }

        public ActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }

        public IConfiguration Configuration { get; set; }

        public IWebHostEnvironment WebHostEnvironment { get; set; }

        private CashFlowContext CashFlowContext { get; set; }

        public IProxyServices IProxyServices => new ProxyServices(CashFlowContext);

        public CryptoServices CryptoServices => new CryptoServices();

        public ControllerBase() { }

        public ControllerBase(CashFlowContext cashflowContext)
        {
            CashFlowContext = cashflowContext;
        }

        public ControllerBase(IOptions<ApplicationConfig> appConfOptions)
        {
            AppConfigOptions = appConfOptions.Value;
        }

        public ControllerBase(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public ControllerBase(IOptions<ApplicationConfig> appConfOptions, CashFlowContext cashflowContext)
        {
            AppConfigOptions = appConfOptions.Value;
            CashFlowContext = cashflowContext;
        }

        public ControllerBase(IOptions<ApplicationConfig> appConfOptions, IWebHostEnvironment webHostEnvironment)
        {
            AppConfigOptions = appConfOptions.Value;
            WebHostEnvironment = webHostEnvironment;
        }

        public ControllerBase(IWebHostEnvironment webHostEnvironment, CashFlowContext cashflowContext)
        {
            WebHostEnvironment = webHostEnvironment;
            CashFlowContext = cashflowContext;
        }

        public ControllerBase(IOptions<ApplicationConfig> appConfOptions, IWebHostEnvironment webHostEnvironment, CashFlowContext cashflowContext)
        {
            AppConfigOptions = appConfOptions.Value;
            WebHostEnvironment = webHostEnvironment;
            CashFlowContext = cashflowContext;
        }
    }
}
