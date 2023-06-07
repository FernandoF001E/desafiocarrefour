using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.General
{
    public enum ECodeReturn
    {
        SUCCESS = 0,
        INVALID_PARAMETER = -1,
        UNDEFINED_ERROR = -2,
        ACTION_NOT_PERFORMED = -3,
        SEARCH_ERROR = -4,
        NOTFOUND = -5,
        PARAMETER_CAN_NOT_NULL = -6,
        UNVERIFIED_ACCOUNT = -7,
        INVALID_AUTHENTICATION = -8,
        SESSION_EXPIRED = -9,
        INVALID_TOKEN = -10,
        INVALID_MODEL = -11,
        RECORDALREADYEXISTS = -12,
        RECORDSELECT = -13,

        UNAUTHORIZED = -401,
        FORBIDDEN = -403,
        NOTACCEPTABLE = -406,

        INVALID_APPICATIONKEY = -400,

        EXCEPTION = -99,
        INVALID_DATA = -98
    }
}
