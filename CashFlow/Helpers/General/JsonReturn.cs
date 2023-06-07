using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.General
{
    public class JsonReturn<T> where T : class, new()
    {
        public ECodeReturn? Code { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public List<T>? ListData { get; set; }

        public Exception? ResultException { get; set; }

        public void SetSuccess()
        {
            Code = ECodeReturn.SUCCESS;
            Message = "Success";
        }

        public void SetSuccess(string message)
        {
            Code = ECodeReturn.SUCCESS;
            Message = message;
        }

        public void SetSuccess(string message, List<T> obj)
        {
            Code = ECodeReturn.SUCCESS;
            Message = message;

            if (obj != null)
            {
                ListData = (List<T>)obj;
            }
        }

        public void SetSuccess(string message, T obj)
        {
            Code = ECodeReturn.SUCCESS;
            Message = message;

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetSuccess(List<T> obj)
        {
            Code = ECodeReturn.SUCCESS;
            Message = "Success";

            if (obj != null)
            {
                ListData = (List<T>)obj;
            }
        }

        public void SetSuccess(T obj)
        {
            Code = ECodeReturn.SUCCESS;
            Message = "Success";

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetRecordAlreadyExists(string message)
        {
            Code = ECodeReturn.RECORDALREADYEXISTS;
            Message = message;
        }

        public void SetInvalidData(string message)
        {
            Code = ECodeReturn.INVALID_DATA;
            Message = message;
        }

        public void SetWarning(string message)
        {
            Code = ECodeReturn.RECORDALREADYEXISTS;
            Message = message;
        }

        public void SetInvalidParameter()
        {
            SetInvalidParameter(null);
        }

        public void SetInvalidParameter(T obj)
        {
            Code = ECodeReturn.INVALID_PARAMETER;
            Message = "Invalid parameter";

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetUndefinedError(T obj, string message)
        {
            Code = ECodeReturn.UNDEFINED_ERROR;
            Message = message;

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetActionNotPerformed(T obj)
        {
            Code = ECodeReturn.ACTION_NOT_PERFORMED;
            Message = "Action not performed";

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetActionNotPerformed()
        {
            Code = ECodeReturn.ACTION_NOT_PERFORMED;
            Message = "Action not performed";
        }

        public void SetErrorSearch(T obj)
        {
            Code = ECodeReturn.SEARCH_ERROR;
            Message = "Error fetching entity";

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetNotFound(T obj)
        {
            Code = ECodeReturn.NOTFOUND;
            Message = "Entity not found";

            if (obj != null)
            {
                Data = (T)obj;
            }
        }

        public void SetNotFound(string message)
        {
            Code = ECodeReturn.NOTFOUND;
            Message = message;
        }

        public void SetParmeterNotNull(T obj)
        {
            Code = ECodeReturn.PARAMETER_CAN_NOT_NULL;
            Message = "Parameter submitted can not be null";

            if (obj != null)
            {
                Data = (T)obj;
            }
        }
        public void SetInvalidToken()
        {
            Code = ECodeReturn.INVALID_TOKEN;
            Message = "Invalid token";
        }

        public void SetInvalidAuthentication()
        {
            Code = ECodeReturn.INVALID_AUTHENTICATION;
            Message = "Invalid authentication";
        }
        public void SetSessionExpired()
        {
            Code = ECodeReturn.SESSION_EXPIRED;
            Message = "Sua sessão expirou! Você será redirecionado para o Login em 7 segundos.";
        }

        public void SetAlreadyRegistered()
        {
            Code = ECodeReturn.NOTACCEPTABLE;
            Message = "Data already registered";
        }

        public void SetException(Exception ex)
        {
            SetException(ex, null);
        }

        public void SetException(Exception ex, T obj)
        {
            SetException(ex, obj, false);
        }

        public void SetException(Exception ex, T obj, bool IsShowExceptionDetails)
        {
            Code = ECodeReturn.EXCEPTION;
            Message = ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(); 

            if (obj != null)
            {
                Data = (T)obj;
            }

            if (!IsShowExceptionDetails)
            {
                ResultException = new Exception(ex.Message);
            }
            else ResultException = ex;
        }
    }
}
