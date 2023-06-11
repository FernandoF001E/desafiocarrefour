using CashFlow.Context;
using CashFlow.Data;
using Helpers.General;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IO;
using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CashFlow.Model;
using System.Collections.Generic;
using CashFlow.Reports.Model;
using WebApp.Helpers;

namespace WebApp.Controllers.Reports
{
    public class ReportsController : ControllerBase
    {
        private readonly CashFlowContext _cashflowContext;
        private readonly IWebHostEnvironment IWebHostEnvironment;
        public ReportsController(IWebHostEnvironment iwebHostEnvironment, CashFlowContext cashflowContext) : base(iwebHostEnvironment, cashflowContext)
        {
            _cashflowContext = cashflowContext;
            IWebHostEnvironment = iwebHostEnvironment;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                return LogOff();
            }
            else
            {
                return View(PathViews.FinancialRecordsReports);
            }
        }

        [HttpPost]
        public JsonResultSummary<FinancialRecords> FinancialRecordsReport()
        {
            List<FinancialRecords> listFinancialRecords = null;
            JsonResultSummary<FinancialRecords> summary = new();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionUserKey)))
            {
                summary.SetSessionExpired();
            }
            else
            {
                FinancialRecordsInputFilter financialrecordsInputFilterInput = new();
                var dateRecords = "";
                if (!string.IsNullOrEmpty(Request.Form["searchDate"].ToString().Trim()))
                {
                    dateRecords = Request.Form["searchDate"].ToString().Trim();
                    dateRecords = dateRecords.Substring(6, 4) + "-" + dateRecords.Substring(3, 2) + "-" + dateRecords[..2];
                }
                financialrecordsInputFilterInput.DateRecords = string.IsNullOrEmpty(dateRecords.ToString().Trim()) ? null : Convert.ToDateTime(dateRecords.ToString().Trim());
                
                try
                {
                    TableInputFilter inputTable = new()
                    {
                        Limit = 9999, //Sem paginação
                        Index = 1     //sem paginação
                    };

                    IEnumerable<FinancialRecords> result = IProxyServices.CashFlow.FinancialRecords.GetAllReport(financialrecordsInputFilterInput, inputTable.Limit, inputTable.Index, out _countData, out _totalPages);
                    if (result != null)
                    {
                        listFinancialRecords = new List<FinancialRecords>();

                        foreach (FinancialRecords obj in result)
                        {
                            listFinancialRecords.Add(new FinancialRecords(obj.FinancialRecordsId, obj.Description, obj.DateRecords, obj.FinancialValue, obj.RecordType, obj.Observation));
                        }
                    }

                    var xPath = IWebHostEnvironment.ContentRootPath + "\\Reports\\relatorio" + dateRecords + ".pdf";
                    Document doc = new();
                    PdfPTable tableLayout = new(5);
                    
                    PdfWriter.GetInstance(doc, new FileStream(xPath, FileMode.Create));
                    FinancialRecordsReports financialrecordsReports = new();
                    doc.Open();
                    
                    doc.Add(financialrecordsReports.Add_Content_To_PDF(tableLayout, listFinancialRecords));
                    
                    doc.Close();
                }
                catch (Exception ex)
                {
                    summary = new JsonResultSummary<FinancialRecords>(ex);
                    Log.Error(ex, "Error Report FinancialRecords");
                }
            }
            return summary;
        }
    }
}
