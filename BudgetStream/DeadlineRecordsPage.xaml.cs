using Firebase.Database;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using Firebase;
using Firebase.Database.Query;

namespace BudgetStream;

public partial class DeadlineRecordsPage : ContentPage
{
    FirebaseHelper firebaseHelper = new FirebaseHelper();
    FirebaseClient firebaseClient = new FirebaseClient("https://mobile-app-development-60d7a-default-rtdb.asia-southeast1.firebasedatabase.app/");

    public DeadlineRecordsPage()
    {
        InitializeComponent();

        // Load deadline records when the page appears
        this.Appearing += async (sender, e) =>
        {
            await LoadDeadlineRecords();
        };
    }

    // Method to load deadline records from Firebase database
    private async Task LoadDeadlineRecords()
    {
        try
        {
            // Read deadline records from Firebase database
            var deadlineRecords = await firebaseHelper.GetDeadlineRecords();

            // Bind the list of deadline records to the ListView
            deadlineListView.ItemsSource = deadlineRecords;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading deadline records from Firebase: {ex.Message}");
        }
    }
    private async void DeleteRecordButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Get the button that was clicked
            ImageButton button = (ImageButton)sender;

            // Get the key value from the CommandParameter
            string recordKeyToDelete = button.CommandParameter as string;

            // Call the method to delete the record using the key value
            await firebaseHelper.DeleteRecordFromFirebase(recordKeyToDelete);

            // Reload the deadline records after deletion
            await LoadDeadlineRecords();
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the deletion process
            Console.WriteLine($"Error deleting record: {ex.Message}");
        }
    }

}


