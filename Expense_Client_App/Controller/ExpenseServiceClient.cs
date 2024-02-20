using Expense_Client_App.ExpenseServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Client_App.Controller
{
    public class ExpenseServiceClient
    {
        private readonly ExpenseServiceClient _client;

        public ExpenseServiceClient()
        {
            _client = new ExpenseServiceClient();
        }

        public List<Expense> GetExpenses()
        {
            return _client.GetExpenses().ToList();
        }

        public void AddExpense(Expense expense)
        {
            _client.AddExpense(expense);
        }

        public void UpdateExpense(Expense updatedExpense)
        {
            Expense existingExpense = _client.GetExpenses().FirstOrDefault(e => e.ExpenseId == updatedExpense.ExpenseId);
            if (existingExpense != null)
            {
                existingExpense.Description = updatedExpense.Description;
                existingExpense.Amount = updatedExpense.Amount;
                // Update other properties as needed

                _client.UpdateExpense(existingExpense);
            }
            else
            {
                Console.WriteLine($"Expense with ID {updatedExpense.ExpenseId} not found.");
            }
        }

        public void DeleteExpense(int expenseId)
        {
            Expense expenseToDelete = _client.GetExpenses().FirstOrDefault(e => e.ExpenseId == expenseId);
            if (expenseToDelete != null)
            {
                _client.DeleteExpense(expenseToDelete.ExpenseId);
            }
            else
            {
                Console.WriteLine($"Expense with ID {expenseId} not found.");
            }
        }
    }
}
