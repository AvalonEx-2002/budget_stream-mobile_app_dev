using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetStream
{
    public class DeadlineRecord
    {

        public string Title { get; set; }

        public double AmountDue { get; set; }

        public DateTime DeadlineDate { get; set; }

        public string TransactionType { get; set; }

        // Add a property to store the unique key
        public string Key { get; set; }
    }
}
