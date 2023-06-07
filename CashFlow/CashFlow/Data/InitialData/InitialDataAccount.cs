using Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Data
{
    public class InitialDataAccount
    {
        public static Account[] Account = new Account[]
        {
            new Account
            {
                AccountId = 1,
                Name = "Teste Carrefour",
                Email = "carrefour@teste.com.br",
                Password = "2A9FA926BA9A2696ECAAF9B344A8E5DDC5673CB412F9919F0BA9AEE6735C562718EF8AB45F509C70C1048D2E734AC0782B401026C3967211FD766CD861AEE03E", //f231206e
                RecordStatus = EStatus.Active,
                InsertDate = DateTime.Now
            }
        };
    }
}
