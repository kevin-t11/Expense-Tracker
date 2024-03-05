using System;
using System.Runtime.Serialization;

namespace ExpenseTracker_WCF
{
    [DataContract]
    public class Expense
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime Date { get; set; }
    }
}
