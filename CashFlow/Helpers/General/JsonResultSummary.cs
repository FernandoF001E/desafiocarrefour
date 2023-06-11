using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.General
{
    public class JsonResultSummary<T> where T : class, new()
    {
        public ECodeReturn? Code { get; set; }

        public string? Message { get; set; }
        
        public string? XPath { get; set; }

        public int? Count { get; set; }

        public int? TotalPages { get; set; }

        public int? PagingLimit { get; set; }

        public int? PagingIndex { get; set; }

        public IEnumerable<T>? Data { get; set; }

        public Exception? ResultException { get; set; }
  
        public JsonResultSummary() { }

        public JsonResultSummary(string message)
        {
            Code = ECodeReturn.RECORDSELECT;
            Message = message;
        }

        public JsonResultSummary(IEnumerable<T> dataList)
        {
            Code = ECodeReturn.SUCCESS;
            Message = "Success";

            if (dataList != null)
            {
                Data = dataList;
                Count = dataList.Count<T>();
            }
        }

        public JsonResultSummary(IEnumerable<T> dataList, int limit, int index, int totalData, int totalPages)
        {
            Code = ECodeReturn.SUCCESS;
            Message = "Success";

            if (dataList != null)
            {
                Data = dataList;
            }

            Count = totalData;
            TotalPages = totalPages;
            PagingLimit = limit;
            PagingIndex = index;
        }

        public JsonResultSummary(IEnumerable<T> dataList, int limit, int index, int totalData, int totalPages, string xPath)
        {
            Code = ECodeReturn.SUCCESS;
            Message = "Success";

            if (dataList != null)
            {
                Data = dataList;
            }

            Count = totalData;
            TotalPages = totalPages;
            PagingLimit = limit;
            PagingIndex = index;
            XPath = xPath;
        }

        public JsonResultSummary(Exception ex)
        {
            SetException(ex, false);
        }

        public JsonResultSummary(Exception ex, bool IsShowExceptionDetails)
        {
            SetException(ex, IsShowExceptionDetails);
        }

        private void SetException(Exception ex, bool IsShowExceptionDetails)
        {
            Code = ECodeReturn.EXCEPTION;
            Message = "Ops! An exception occurred. Please check the application log";

            if (!IsShowExceptionDetails)
            {
                ResultException = new Exception(ex.Message);
            }
            else ResultException = ex;
        }

        public void SetInvalidToken()
        {
            Code = ECodeReturn.INVALID_TOKEN;
            Message = "Invalid token";
        }

        public void SetSessionExpired()
        {
            Code = ECodeReturn.SESSION_EXPIRED;
            Message = "Sua sessão expirou! Você será redirecionado para o Login em 7 segundos.";
        }

        public void SetNotFound()
        {
            Code = ECodeReturn.NOTFOUND;
            Message = "Entity not found";
        }
    }
}
