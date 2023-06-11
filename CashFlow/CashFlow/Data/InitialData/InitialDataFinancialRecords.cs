using Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Data.InitialData
{
    public class InitialDataFinancialRecords
    {
        public static FinancialRecords[] FinancialRecords = new FinancialRecords[]
        {
            new FinancialRecords { FinancialRecordsId = 1, Description = "Mercado", DateRecords = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")), FinancialValue = 500, InsertDate = DateTime.Now, RecordType = ERecordType.Debit, RecordStatus = EStatus.Active},
            new FinancialRecords { FinancialRecordsId = 2, Description = "Loja", DateRecords = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")), FinancialValue = 600, InsertDate = DateTime.Now, RecordType = ERecordType.Debit, RecordStatus = EStatus.Active},
            new FinancialRecords { FinancialRecordsId = 3, Description = "Farmácia", DateRecords = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")), FinancialValue = 1200, InsertDate = DateTime.Now, RecordType = ERecordType.Debit, RecordStatus = EStatus.Active},
            new FinancialRecords { FinancialRecordsId = 4, Description = "Salario", DateRecords = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")), FinancialValue = 10000, InsertDate = DateTime.Now, RecordType = ERecordType.Credit, RecordStatus = EStatus.Active}
        };
    }
}
