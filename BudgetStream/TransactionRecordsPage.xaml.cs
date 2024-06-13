using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetStream
{
    public partial class TransactionRecordsPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            LoadTransactionRecords();
        }

        public TransactionRecordsPage()
        {
            InitializeComponent();
        }

        private async Task LoadTransactionRecords()
        {
            try
            {
                var transactions = await firebaseHelper.GetTransactionRecords();

                // Sort transactions by date in descending order
                transactions = transactions.OrderByDescending(t => t.DateTime).ToList();

                transactionListView.ItemsSource = transactions;

                // Calculate summary statistics
                decimal totalIncome = transactions.Where(t => t.TransactionType == "Income (+)").Sum(t => t.Amount);
                decimal totalExpenses = transactions.Where(t => t.TransactionType == "Expense (-)").Sum(t => t.Amount);
                decimal netIncome = totalIncome - totalExpenses;

                /*
                // Display summary statistics
                summaryLabel.Text = $"Total Income: RM {totalIncome}\nTotal Expenses: RM {totalExpenses}\nNet Income: RM {netIncome}";
                */

                totalIncomeLabel.Text = $"Total Income : RM {totalIncome.ToString("F2")}";
                totalExpensesLabel.Text = $"Total Expenses : RM {totalExpenses.ToString("F2")}";
                netIncomeLabel.Text = $"Net Savings : RM {netIncome.ToString("F2")}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transaction records: {ex.Message}");
            }
        }
    }
}
