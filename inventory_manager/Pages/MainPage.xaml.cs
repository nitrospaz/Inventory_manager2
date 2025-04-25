using inventory_manager.Models;
using inventory_manager.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace inventory_manager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private InventoryDataService _dataAccess;
        private int? _editingItemId = null;

        public MainPage()
        {
            this.InitializeComponent();
            ResetInputFields();
            _dataAccess = new InventoryDataService();
            refreshList();
        }

        private void Output_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateListViewMaxHeight();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateListViewMaxHeight();
        }

        private void UpdateListViewMaxHeight()
        {
            // if I dont set the max height then the scroll bars act weird.
            if (Output != null)
            {
                var outputContainer = Output as UIElement;
                if (outputContainer != null)
                {
                    var appWindow = App.AppWindow;
                    if (appWindow != null)
                    {
                        var appWindowHeight = appWindow.Size.Height;
                        var topOfListView = outputContainer.TransformToVisual(this).TransformPoint(new Point(0, 0)).Y;
                        // I dont know why 80 but that makes it right.
                        var expectedHeight = appWindowHeight - topOfListView - 80;
                        if (expectedHeight > 200)
                        {
                            Output.MaxHeight = expectedHeight;
                        }
                        else
                        {
                            Output.MaxHeight = 200;
                        }
                    }
                }
            }
        }

        private void ResetInputFields()
        {
            NameInput.Text = string.Empty;
            DescriptionInput.Text = string.Empty;
            QuantityInput.Text = "1";
            PriceInput.Text = "0.00";
            DateAddedInput.Text = DateTime.Now.ToString("MM/dd/yyyy");
            _editingItemId = null;
            NameInput.Tag = null;
            ActionButton.Content = "Add New";
        }

        private void Output_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Cast the clicked item to InventoryItem
            InventoryItem clickedItem = (InventoryItem)e.ClickedItem;

            // Populate the input fields with the clicked item's data
            NameInput.Text = clickedItem.Name;
            DescriptionInput.Text = clickedItem.Description;
            QuantityInput.Text = clickedItem.Quantity.ToString();
            PriceInput.Text = clickedItem.Price.ToString();
            DateAddedInput.Text = clickedItem.DateAdded.ToString("MM/dd/yyyy");

            // Store the clicked item's Id for later use
            NameInput.Tag = clickedItem.Id;
            _editingItemId = clickedItem.Id;

            // Change the button content to "Update"
            ActionButton.Content = "Update";
        }

        //private void SetDefaultValues()
        //{
        //    //NameInput.Text = "Default Name";
        //    //DescriptionInput.Text = "Default Description";
        //    QuantityInput.Text = "1";
        //    PriceInput.Text = "0.00";
        //    DateAddedInput.Text = DateTime.Now.ToString("MM/dd/yyyy");
        //    NameInput.Tag = null;
        //    _editingItemId = null;
        //}

        private async void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse input values
                string name = NameInput.Text;
                string description = DescriptionInput.Text;
                if (!int.TryParse(QuantityInput.Text, out int quantity))
                {
                    ShowError("Quantity must be a valid integer.");
                    return;
                }
                if (!decimal.TryParse(PriceInput.Text, out decimal price))
                {
                    ShowError("Price must be a valid decimal number.");
                    return;
                }
                if (!DateTime.TryParse(DateAddedInput.Text, out DateTime dateAdded))
                {
                    ShowError("Date Added must be a valid date.");
                    return;
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Name is required.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(description))
                {
                    ShowError("Description is required.");
                    return;
                }

                if (_editingItemId.HasValue)
                {
                    // Update the existing item
                    InventoryItem updatedItem = new InventoryItem(_editingItemId.Value, name, description, quantity, price, dateAdded);
                    await _dataAccess.UpdateInventoryItemAsync(updatedItem);
                    _editingItemId = null;
                    ActionButton.Content = "Add New";
                }
                else
                {
                    // Create a new InventoryItem
                    InventoryItem newItem = new InventoryItem(0, name, description, quantity, price, dateAdded);
                    await _dataAccess.AddInventoryItemAsync(newItem);
                }

                refreshList();
                ResetInputFields();
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async void DeleteData(object sender, RoutedEventArgs e)
        {
            // Get the Id of the item to delete from the Tag property of the button
            int id = (int)((Button)sender).Tag;

            // Delete the item from the database
            await _dataAccess.DeleteInventoryItemAsync(id);
            refreshList();
        }

        private void ShowError(string message)
        {
            // Display an error message to the user
            var dialog = new ContentDialog
            {
                Title = "Input Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot // Set the XamlRoot property
            };
            _ = dialog.ShowAsync();
        }

        private void refreshList()
        {
            // Get the list of InventoryItems and set it as the ItemsSource for the ListView
            Output.ItemsSource = _dataAccess.GetInventoryItems();
        }
    }
}
