using CashFlow.Context;
using CashFlow.Data;
using Helpers.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System;
using WebApp.Helpers;
using CashFlow.Model;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Controllers.Common
{
    public class FinancialRecordsController : ControllerBase
    {
        private readonly CashFlowContext _cashflowContext;
        public FinancialRecordsController(CashFlowContext cashflowContext) : base(cashflowContext)
        {
            _cashflowContext = cashflowContext;
        }

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

        [HttpPost]
        public JsonResultSummary<FinancialRecords> Search()
        {
            JsonResultSummary<FinancialRecords> summary = new();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                summary.SetSessionExpired();
            }
            else
            {
                FinancialRecordsInputFilter financialrecordsInputFilterInput = new()
                {
                    Description = Request.Form["searchDescription"].ToString().Trim()
                };
                var dateRecords = "";
                if (!string.IsNullOrEmpty(Request.Form["searchDate"].ToString().Trim()))
                {
                    dateRecords = Request.Form["searchDate"].ToString().Trim();
                    dateRecords = dateRecords.Substring(6, 4) + "-" + dateRecords.Substring(3, 2) + "-" + dateRecords[..2];
                }
                financialrecordsInputFilterInput.DateRecords = string.IsNullOrEmpty(dateRecords.ToString().Trim()) ? null : Convert.ToDateTime(dateRecords.ToString().Trim());
                financialrecordsInputFilterInput.RecordType = string.IsNullOrEmpty(Request.Form["searchRecordType"].ToString().Trim()) ? 0 : (ERecordType)Convert.ToInt32(Request.Form["searchRecordType"].ToString().Trim());

                try
                {
                    TableInputFilter inputTable = new()
                    {
                        Limit = Convert.ToInt32(Request.Form["pageLimits"].ToString().Trim()),
                        Index = Convert.ToInt32(Request.Form["pageTable"].ToString().Trim())
                    };

                    IEnumerable<FinancialRecords> result = IProxyServices.CashFlow.FinancialRecords.GetAllCRUD(financialrecordsInputFilterInput, inputTable.Limit, inputTable.Index, out _countData, out _totalPages);
                    summary = new JsonResultSummary<FinancialRecords>(result, inputTable.Limit, inputTable.Index, PagingCount, PagingTotal);
                }
                catch (Exception ex)
                {
                    summary = new JsonResultSummary<FinancialRecords>(ex);
                    Log.Error(ex, "Error Search Account");
                }
            }
            return summary;
        }

        [HttpPost]
        public async Task<JsonReturn<FinancialRecords>> Add()
        {
            FinancialRecords obj = null;
            JsonReturn<FinancialRecords> result = new();
            using var transaction = _cashflowContext.Database.BeginTransaction();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                transaction.Rollback();
                result.SetSessionExpired();
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.Form["hdnCode"].ToString().Trim()) && Convert.ToInt32(Request.Form["hdnCode"].ToString().Trim()) > 0)
                        obj = await IProxyServices.CashFlow.FinancialRecords.Get(Convert.ToInt32(Request.Form["hdnCode"].ToString().Trim()));

                    obj ??= new FinancialRecords();

                    obj.Description = Request.Form["txtDescription"].ToString().Trim();
                    obj.FinancialValue = Convert.ToDecimal(Request.Form["txtValue"].ToString().Trim());
                    obj.DateRecords = Convert.ToDateTime(Request.Form["txtDate"].ToString().Trim());
                    obj.RecordType = (ERecordType)Convert.ToInt32(Request.Form["cboType"].ToString().Trim());
                    obj.Observation = Request.Form["txtObservation"].ToString().Trim();

                    if (obj.FinancialRecordsId == 0)
                    {
                        obj.RecordStatus = EStatus.Active;
                        obj.InsertDate = DateTime.Now;
                        obj = await IProxyServices.CashFlow.FinancialRecords.Add(obj);
                    }
                    else
                    {
                        obj.UpdateDate = DateTime.Now;
                        obj = await IProxyServices.CashFlow.FinancialRecords.Update(obj);
                    }
                    transaction.Commit();
                    result.SetSuccess(obj);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.SetException(ex, obj);
                    Log.Error(ex, "Error save Account");
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<JsonReturn<FinancialRecords>> Load()
        {
            FinancialRecords obj = new();
            JsonReturn<FinancialRecords> result = new();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                result.SetSessionExpired();
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.Form["id"].ToString().Trim()))
                    {
                        obj = await IProxyServices.CashFlow.FinancialRecords.Get(Convert.ToInt32(Request.Form["id"].ToString().Trim()));
                    }

                    result.SetSuccess(obj);
                }
                catch (Exception ex)
                {
                    result.SetException(ex, obj);
                    Log.Error(ex, "Error Load FinancialRecords");
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<JsonReturn<FinancialRecords>> ChangeStatus()
        {
            FinancialRecords obj = null;
            JsonReturn<FinancialRecords> result = new();
            using var transaction = _cashflowContext.Database.BeginTransaction();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                transaction.Rollback();
                result.SetSessionExpired();
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.Form["id"].ToString().Trim()) && !string.IsNullOrEmpty(Request.Form["status"].ToString().Trim()))
                    {
                        obj = await IProxyServices.CashFlow.FinancialRecords.Get(Convert.ToInt32(Request.Form["id"].ToString().Trim()));

                        if (obj != null)
                        {
                            obj.RecordStatus = (EStatus)Convert.ToInt32(Request.Form["status"].ToString().Trim());
                            obj.UpdateDate = DateTime.Now;
                            obj = await IProxyServices.CashFlow.FinancialRecords.Update(obj);

                            transaction.Commit();
                            result.SetSuccess(obj);
                        }
                        else
                        {
                            transaction.Rollback();
                            result.SetNotFound(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.SetException(ex, obj);
                    Log.Error(ex, "Error ChangeStatus FinancialRecords");
                }
            }
            return result;
        }
    }
}
