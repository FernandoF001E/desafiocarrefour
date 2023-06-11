using AbstractModel.EFCore;
using CashFlow.Context;
using CashFlow.Data;
using CashFlow.Model;
using Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.CRUD
{
    public class CRUDFinancialRecords : AbstractContext<FinancialRecords>
    {
        internal CRUDFinancialRecords(CashFlowContext context) : base(context) { }

        public IEnumerable<FinancialRecords> GetAllCRUD(FinancialRecordsInputFilter input, int limit, int index, out int total, out int totalPage)
        {
            var query = GetAllCRUD(input);

            total = query.Count();
            totalPage = (int)Math.Ceiling((double)total / limit);

            return query
                    .OrderBy(s => s.FinancialRecordsId)
                    .Skip((index - 1) * limit)
                    .Take(limit)
                    .ToList();
        }

        public IQueryable<FinancialRecords> GetAllCRUD(FinancialRecordsInputFilter input)
        {
            var query = from a in _context.Set<FinancialRecords>()
                        .Where(t => string.IsNullOrEmpty(input.Description) || t.Description.ToLower().Contains(input.Description.ToLower()))
                        .Where(t => input.DateRecords == null || t.DateRecords == input.DateRecords)
                        .Where(t => input.RecordType == 0 || t.RecordType == input.RecordType)

                        select new FinancialRecords()
                        {
                            FinancialRecordsId = a.FinancialRecordsId,
                            Description = a.Description,
                            DateRecords = a.DateRecords,
                            FinancialValue = a.FinancialValue,
                            RecordType = a.RecordType,
                            Observation = a.Observation,
                            RecordStatus = a.RecordStatus,
                            InsertDate = a.InsertDate,
                            UpdateDate = a.UpdateDate
                        };

            return query.OrderBy(s => s.FinancialRecordsId);
        }

        public IEnumerable<FinancialRecords> GetAllReport(FinancialRecordsInputFilter input, int limit, int index, out int total, out int totalPage)
        {
            var query = GetAllReport(input);

            total = query.Count();
            totalPage = (int)Math.Ceiling((double)total / limit);

            return query
                    .OrderBy(s => s.DateRecords)
                    .Skip((index - 1) * limit)
                    .Take(limit)
                    .ToList();
        }

        public IQueryable<FinancialRecords> GetAllReport(FinancialRecordsInputFilter input)
        {
            var query = from a in _context.Set<FinancialRecords>()
                        .Where(t => string.IsNullOrEmpty(input.Description) || t.Description.ToLower().Contains(input.Description.ToLower()))
                        .Where(t => input.DateRecords == null || t.DateRecords == input.DateRecords)
                        .Where(t => input.RecordType == 0 || t.RecordType == input.RecordType)
                        .Where(t => t.RecordStatus == EStatus.Active)

                        select new FinancialRecords()
                        {
                            FinancialRecordsId = a.FinancialRecordsId,
                            Description = a.Description,
                            DateRecords = a.DateRecords,
                            FinancialValue = a.FinancialValue,
                            RecordType = a.RecordType,
                            Observation = a.Observation,
                            RecordStatus = a.RecordStatus,
                            InsertDate = a.InsertDate,
                            UpdateDate = a.UpdateDate
                        };

            return query.OrderBy(s => s.DateRecords);
        }
    }
}
