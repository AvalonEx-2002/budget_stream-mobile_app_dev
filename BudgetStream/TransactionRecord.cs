using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetStream
{
    public class TransactionRecord
    {
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public DateTime DateTime { get; set; }
        public string TransactionType { get; set; }
        public string Comments { get; set; }
    }
}
