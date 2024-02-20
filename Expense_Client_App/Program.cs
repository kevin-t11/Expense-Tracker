using Expense_Client_App.ExpenseServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expense_Client_App
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using (ExpenseServiceClient client = new ExpenseServiceClient())
                {
                    // Example: Get and display expenses
                    Expense[] expensesArray = client.GetExpenses();
                    List<Expense> expensesList = expensesArray.ToList();
                    DisplayExpenses(expensesList);

                    // Example: Add a new expense
                    Expense newExpense = new Expense { Description = "New Expense", Amount = 100.0M };
                    client.AddExpense(newExpense);

                    // Example: Update an existing expense
                    Expense expenseToUpdate = expensesList.FirstOrDefault();
                    if (expenseToUpdate != null)
                    {
                        expenseToUpdate.Amount = 150.0M;
                        client.UpdateExpense(expenseToUpdate);
                    }

                    // Example: Delete an existing expense
                    Expense expenseToDelete = expensesList.FirstOrDefault();
                    if (expenseToDelete != null)
                    {
                        client.DeleteExpense(expenseToDelete.ExpenseId);
                    }

                    // Display updated list of expenses
                    expensesArray = client.GetExpenses();
                    expensesList = expensesArray.ToList();
                    DisplayExpenses(expensesList);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

           
        }

        static void DisplayExpenses(List<Expense> expenses)
        {
            Console.WriteLine("List of Expenses:");
            foreach (var expense in expenses)
            {
                Console.WriteLine($"ExpenseId: {expense.ExpenseId}, Description: {expense.Description}, Amount: {expense.Amount}");
            }
        }
    }
}
