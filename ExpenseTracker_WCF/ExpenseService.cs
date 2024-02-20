using ExpenseTracker_WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


// ExpenseService.cs (Service Implementation)


namespace ExpenseTracker_WCF
{
    public class ExpenseService : IExpenseService
    {

        private List<Expense> expenses;

        public ExpenseService()
        {
            expenses = new List<Expense>();
        }

        public List<Expense> GetExpenses()
        {
            return expenses;
        }

        public void AddExpense(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense), "Expense cannot be null.");
            }

            expense.ExpenseId = GenerateExpenseId();
            expenses.Add(expense);
        }

        public void UpdateExpense(Expense updatedExpense)
        {
            if (updatedExpense == null)
            {
                throw new ArgumentNullException(nameof(updatedExpense), "Updated expense cannot be null.");
            }

            Expense existingExpense = expenses.FirstOrDefault(e => e.ExpenseId == updatedExpense.ExpenseId);
            if (existingExpense != null)
            {
                existingExpense.Description = updatedExpense.Description;
                existingExpense.Amount = updatedExpense.Amount;
                // Update other properties as needed
            }
            else
            {
                throw new ArgumentException($"Expense with ID {updatedExpense.ExpenseId} not found.", nameof(updatedExpense));
            }
        }

        public void DeleteExpense(int expenseId)
        {
            Expense expenseToDelete = expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
            if (expenseToDelete != null)
            {
                expenses.Remove(expenseToDelete);
            }
            else
            {
                throw new ArgumentException($"Expense with ID {expenseId} not found.", nameof(expenseId));
            }
        }
        private int GenerateExpenseId()
        {
            // Generate a unique expense ID based on your requirements
            // This can be an incrementing counter, a GUID, or any other suitable method
            // For simplicity, we'll use a basic incrementing counter in this example
            return expenses.Count + 1;
        }
    }
}



