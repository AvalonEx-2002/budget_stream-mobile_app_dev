using Firebase;
using Firebase.Database;
using Firebase.Database.Query;

namespace BudgetStream
{
    public partial class DeadlineFormPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        FirebaseClient firebase = new FirebaseClient("https://mobile-app-development-60d7a-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public DeadlineFormPage()
        {
            InitializeComponent();
        }

        public string GetTransactionTypeText(string transactionType)
        {
            switch (transactionType)
            {
                case "Expense (-)":
                    return "Cash Deduction"; // Markdown bold
                case "Income (+)":
                    return "Cash Addition"; // Markdown italic
                default:
                    return "Unknown"; // Markdown italic
            }
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (!IsValidInput())
                {
                    await DisplayAlert("Error", "Please fill in all required fields", "OK");
                    return;
                }

                // Get input values
                string title = nameEntry.Text;
                double amount = Convert.ToDouble(amountEntry.Text);
                DateTime date = deadlineDatePicker.Date;
                string transactionType = GetTransactionTypeText(transactionTypePicker.SelectedItem.ToString());

                // Create a new DeadlineRecord object
                var newRecord = new DeadlineRecord
                {
                    Title = title,
                    AmountDue = amount,
                    DeadlineDate = date,
                    TransactionType = transactionType,
                    Key = "",
                };

                // Add the record to Firebase and get the key
                string recordKey = await firebaseHelper.AddDeadlineRecord(newRecord);

                // Update the DeadlineRecord object with the key
                newRecord.Key = recordKey;

                // Update the recently added record in Firebase to include the key
                await firebase
                    .Child("DeadlineRecords")
                    .Child(recordKey)
                    .PutAsync(new { 
                        Key = recordKey,
                        Title = title,
                        AmountDue = amount,
                        DeadlineDate = date,
                        TransactionType = transactionType,
                    }); // Update with the data values

                // Display success message
                await DisplayAlert("Success", "Deadline record submitted successfully", "OK");

                // Clear input fields
                ClearFields();
            }
            catch (Exception ex)
            {
                // Handle any errors
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        private bool IsValidInput()
        {
            // Implement input validation logic here
            // Return true if input is valid, false otherwise
            // For example:
            return !string.IsNullOrWhiteSpace(nameEntry.Text)
                && !string.IsNullOrWhiteSpace(amountEntry.Text)
                && deadlineDatePicker.Date != DateTime.MinValue
                && transactionTypePicker.SelectedItem != null;
        }

        private void ClearFields()
        {
            // Clear input fields
            nameEntry.Text = string.Empty;
            amountEntry.Text = string.Empty;
            deadlineDatePicker.Date = DateTime.Today;
            transactionTypePicker.SelectedIndex = -1;
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
