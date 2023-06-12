using CashFlow.Model;
using Helpers.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Data
{
    [Table("financialrecords")]
    public class FinancialRecords
    {
        [Key, Column("financialrecordsid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialRecordsId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("financialvalue")]
        public decimal FinancialValue { get; set; }

        [Column("daterecords")]
        public DateTime DateRecords { get; set; }

        [Column("recordtype")]
        public ERecordType RecordType { get; set; }

        [Column("observation")]
        public string Observation { get; set; }

        [Column("recordstatus")]
        public EStatus RecordStatus { get; set; }

        [Column("insertdate")]
        public DateTime InsertDate { get; set; }

        [Column("updatedate")]
        public DateTime? UpdateDate { get; set; }

        public FinancialRecords() { }

        public FinancialRecords(int financialrecordsid, string description, DateTime dateRecords, decimal financialValue, ERecordType recordType, string observation)
        {
            FinancialRecordsId = financialrecordsid;
            Description = description;
            DateRecords = dateRecords;
            FinancialValue = financialValue;
            RecordType = recordType;
            Observation = observation;
        }
    }
}
