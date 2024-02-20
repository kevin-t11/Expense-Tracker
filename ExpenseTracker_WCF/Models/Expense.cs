using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

// Expense.cs (Expense Model)

namespace ExpenseTracker_WCF
{
    [DataContract]
    public class Expense
    {
        [DataMember]
        public int ExpenseId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        // Add other properties as needed

        public override string ToString()
        {
            return $"ExpenseId: {ExpenseId}, Description: {Description}, Amount: {Amount}";
        }
    }

}
