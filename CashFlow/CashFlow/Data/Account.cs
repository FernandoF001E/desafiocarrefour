using Helpers.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Data
{
    [Table("account")]
    public class Account
    {
        [Key, Column("accountid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("lastaccess")]
        public DateTime? LastAccess { get; set; }

        [Column("recordstatus")]
        public EStatus RecordStatus { get; set; }

        [Column("insertdate")]
        public DateTime InsertDate { get; set; }

        [Column("updatedate")]
        public DateTime? UpdateDate { get; set; }

        public Account() { }

        public Account(int accountid)
        {
            AccountId = accountid;
        }
    }
}
