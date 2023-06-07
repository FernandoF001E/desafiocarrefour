using CashFlow.Context;
using CashFlow.Data;
using Helpers.General;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WebApp.Helpers;

namespace WebApp.Controllers.Anthentication
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthenticationController(IOptions<ApplicationConfig> appOptions, IWebHostEnvironment webHostEnvironment, CashFlowContext cashflowContext) : base(appOptions, webHostEnvironment, cashflowContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            return View(PathViews.Login);
        }

        [HttpPost]
        public async Task<JsonReturn<Account>> Validate()
        {
            Account obj = null;
            JsonReturn<Account> result = new();

            try
            {
                obj = await IProxyServices.CashFlow.Account.FindBy(t => t.Email == Request.Form["txtUser"].ToString().Trim());

                if (obj == null)
                {
                    result.SetNotFound("Usuario não encontrado");
                }
                else
                {
                    if (IProxyServices.CryptoServices.GetSHA512(Request.Form["txtPassword"].ToString().Trim()) == obj.Password)
                    {
                        HttpContext.Session.SetString(SessionUserKey, obj.AccountId.ToString().Trim());

                        Account objAccountLastAcess = await IProxyServices.CashFlow.Account.FindBy(t => t.AccountId == obj.AccountId);
                        objAccountLastAcess.LastAccess = DateTime.Now;
                        objAccountLastAcess = await IProxyServices.CashFlow.Account.Update(objAccountLastAcess);

                        Console.WriteLine("SessionUserKey: " + HttpContext.Session.GetString(SessionUserKey));

                        result.SetSuccess(obj);
                    }
                    else
                    {
                       result.SetNotFound("Usuario/Senha inválida");
                    }

                }
            }
            catch (Exception ex)
            {
                result.SetException(ex, obj);
                Log.Error(ex, "Error Validate Authentication");
            }
            return result;
        }
    }
}
