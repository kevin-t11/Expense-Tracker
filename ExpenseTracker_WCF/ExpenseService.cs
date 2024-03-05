using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;

namespace ExpenseTracker_WCF
{
    public class ExpenseService : IExpenseService
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Expense-Tracker;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private List<Expense> expenses;

        public ExpenseService()
        {
            expenses = new List<Expense>();
        }

        public List<Expense> GetExpenses()
        {
            expenses.Clear(); // Clear existing list before retrieving from the database

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "SELECT Id, Title, Amount, Description, Date FROM Expense";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Expense expense = new Expense
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                Title = reader["Title"].ToString(),
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                Date = reader["Date"] != DBNull.Value ? Convert.ToDateTime(reader["Date"]) : DateTime.MinValue
                            };
                            expenses.Add(expense);
                        }
                    }
                }
            }

            return expenses;
        }

        public void AddExpense(Expense newExpense)
        {
            expenses.Add(newExpense);
            AddExpenseToDatabase(newExpense);
        }

        private void AddExpenseToDatabase(Expense newExpense)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "INSERT INTO Expense (Id, Title, Amount, Description, Date) VALUES (@Id, @Title, @Amount, @Description, @Date)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Provide a value for the 'Id' column
                    command.Parameters.AddWithValue("@Id", newExpense.Id);

                    command.Parameters.AddWithValue("@Title", newExpense.Title);
                    command.Parameters.AddWithValue("@Amount", newExpense.Amount);

                    // Handle null Description by providing a default value
                    if (newExpense.Description != null)
                    {
                        command.Parameters.AddWithValue("@Description", newExpense.Description);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Description", DBNull.Value);
                    }

                    command.Parameters.AddWithValue("@Date", newExpense.Date);

                    command.ExecuteNonQuery();
                }
            }
        }

    public void DeleteExpense(int expenseId)
    {
        if (!ExpenseExists(expenseId))
        {
            // The expense with the specified ID was not found
            throw new FaultException($"Expense with ID {expenseId} not found.");
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "DELETE FROM Expense WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", expenseId);
                    command.ExecuteNonQuery();
                }
            }

            // If we reach here, the deletion was successful
            Console.WriteLine($"Expense with ID {expenseId} deleted successfully.");
        }
        catch (Exception ex)
        {
            // Handle the exception as needed (log, throw, etc.)
            Console.WriteLine($"Error deleting expense with Id {expenseId}: {ex.Message}");
            throw; // Rethrow the exception to indicate failure
        }
    }

    private bool ExpenseExists(int expenseId)
    {
        using (SqlConnection connection = new SqlConnection(cs))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Expense WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", expenseId);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }

        public void UpdateExpense(Expense updatedExpense)
        {
            if (!ExpenseExists(updatedExpense.Id))
            {
                // The expense with the specified ID was not found
                throw new FaultException($"Expense with ID {updatedExpense.Id} not found.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    connection.Open();

                    // Fetch the original expense data from the database
                    string query = "SELECT Id, Title, Amount, Description, Date FROM Expense WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", updatedExpense.Id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Update the fields with new values
                                updatedExpense.Title = reader["Title"].ToString();
                                updatedExpense.Amount = Convert.ToDecimal(reader["Amount"]);
                                updatedExpense.Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;
                                updatedExpense.Date = Convert.ToDateTime(reader["Date"]);
                            }
                            else
                            {
                                throw new FaultException($"Expense with ID {updatedExpense.Id} not found.");
                            }
                        }
                    }

                    // Update the expense in the database
                    UpdateExpenseInDatabase(updatedExpense);

                    Console.WriteLine($"Expense with ID {updatedExpense.Id} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating expense with Id {updatedExpense.Id}: {ex.Message}");
                throw;
            }
        }

        private void UpdateExpenseInDatabase(Expense updatedExpense)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "UPDATE Expense SET Title = @Title, Amount = @Amount, Description = @Description, Date = @Date WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", updatedExpense.Id);
                    command.Parameters.AddWithValue("@Title", updatedExpense.Title);
                    command.Parameters.AddWithValue("@Amount", updatedExpense.Amount);
                    command.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(updatedExpense.Description) ? DBNull.Value : (object)updatedExpense.Description);
                    command.Parameters.AddWithValue("@Date", updatedExpense.Date);

                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
