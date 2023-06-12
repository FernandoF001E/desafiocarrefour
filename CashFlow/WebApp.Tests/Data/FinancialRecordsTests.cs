using CashFlow.Context;
using CashFlow.Data;
using EmptyFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proxy.Services;
using WebApp.Controllers.Common;
using Xunit;

namespace WebApp.Tests.Data
{
    public class FinancialRecordsTests
    {
        private readonly CashFlowContext _cashflowContext;
        public FinancialRecordsTests()
        {
            _cashflowContext = new CashFlowContext();
        }

        [Fact]
        public void ValidateFinancialRescordsAddisCalled_ReturnValid()
        {
            using var context = GetContextWithData();
            using var controller = new FinancialRecordsController(context);

            var result = controller.Add();

            Assert.NotNull(result);
        }

        [Fact]
        public void ValidateFinancialRescordsLoadCalled_ReturnValid()
        {
            using var context = GetContextWithData();
            using var controller = new FinancialRecordsController(context);

            var result = controller.Load();

            Assert.NotNull(result);
        }

        [Fact]
        public void ValidateFinancialRescordsChangeStatusCalled_ReturnValid()
        {
            using var context = GetContextWithData();
            using var controller = new FinancialRecordsController(context);

            var result = controller.ChangeStatus();

            Assert.NotNull(result);
        }

        private static CashFlowContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<CashFlowContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new CashFlowContext(options);

            context.FinancialRecords.Add(new FinancialRecords { FinancialRecordsId = 1, Description = "La Trappe Isid'or", DateRecords = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyy")) });
            context.SaveChanges();

            return context;
        }
    }
}
