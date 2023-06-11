using Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Model
{
    public class FinancialRecordsInputFilter
    {
        public string Description { get; set; }
        public DateTime? DateRecords { get; set; }
        public ERecordType RecordType { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
