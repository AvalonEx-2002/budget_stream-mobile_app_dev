using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetStream
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            // Validate input
            if (!IsValidInput())
            {
                DisplayAlert("Error", "Please fill in all required fields.", "OK");
                return;
            }

            try
            {
                // Get input values
                string amount = amountEntry.Text;
                string purpose = purposeEntry.Text;
                DateTime dateTime = datePicker.Date + timePicker.Time;
                string transactionType = transactionTypePicker.SelectedItem.ToString();
                string comments = commentsEntry.Text;

                // Create a new TransactionRecord object
                TransactionRecord record = new TransactionRecord
                {
                    // Parse the amount string to a double
                    Amount = decimal.Parse(amount),
                    Purpose = purpose,
                    DateTime = dateTime,
                    TransactionType = transactionType,

                    // Check if comments is not null, if true assign comments, otherwise assign "-"
                    Comments = comments != null ? comments : "-"
                };

                // Add the record to Firebase Database
                await firebaseHelper.AddTransactionRecord(record);

                // Display success message
                await DisplayAlert("Success", "Transaction submitted successfully", "OK");

                // Clear input fields
                ClearFields();
            }
            catch (Exception ex)
            {
                // Display error message if there's an exception
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            ClearFields();
        }

        private bool IsValidInput()
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(amountEntry.Text) ||
                string.IsNullOrWhiteSpace(purposeEntry.Text) ||
                datePicker.Date == DateTime.MinValue ||
                transactionTypePicker.SelectedItem == null)
            {
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            // Clear all input fields
            amountEntry.Text = string.Empty;
            purposeEntry.Text = string.Empty;
            datePicker.Date = DateTime.Today;
            timePicker.Time = new TimeSpan(0, 0, 0);
            transactionTypePicker.SelectedIndex = -1;
            commentsEntry.Text = string.Empty;
        }
    }
}
