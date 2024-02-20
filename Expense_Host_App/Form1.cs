using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Expense_Host_App
{
    public partial class Form1 : Form
    {
        private ServiceHost sh = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new ServiceHost instance
            sh = new ServiceHost(typeof(ExpenseTracker_WCF.Expense));

            // Open the service host
            sh.Open();

            // Display a message indicating that the service is running
            label1.Text = "Service is running !!";
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            // Close the service host when the form is closing
            sh.Close();
        }
    }
}
