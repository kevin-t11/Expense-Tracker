using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

// Service Contract Interface implementation

namespace ExpenseTracker_WCF
{
        [ServiceContract]
        public interface IExpenseService
        {
            [OperationContract]
            List<Expense> GetExpenses();

            [OperationContract]
            void AddExpense(Expense expense);

            [OperationContract]
            void UpdateExpense(Expense expense);

            [OperationContract]
            void DeleteExpense(int expenseId);
        }
    
}

