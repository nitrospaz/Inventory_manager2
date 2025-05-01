using inventory_manager.Models;
using inventory_manager.Pages.Dialogs;
using inventory_manager.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace inventory_manager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectConfiguration : Page
    {
        private readonly ProjectDataService _dataService;
        ObservableCollection<Person> People { get; set; }
        ObservableCollection<DeliverableItem> Deliverables { get; set; }

        public ProjectConfiguration()
        {
            this.InitializeComponent();
            _dataService = new ProjectDataService(new InventoryContext());
            People = new ObservableCollection<Person>();
            Deliverables = new ObservableCollection<DeliverableItem>();

            // Bind the collections to the ListViews
            PeopleListView.ItemsSource = People;
            DeliverablesListView.ItemsSource = Deliverables;

            // Load data
            LoadData();
        }

        private async void LoadData()
        {
            // Await the asynchronous methods to get the results
            var people = await _dataService.GetAllPeopleAsync();
            var deliverables = await _dataService.GetAllDeliverableItemsAsync();

            // Clear existing items
            People.Clear();
            Deliverables.Clear();

            // Add new items
            foreach (var person in people)
            {
                People.Add(person);
            }

            foreach (var deliverable in deliverables)
            {
                Deliverables.Add(deliverable);
            }
        }

        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddPersonDialog
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && dialog.Name != null)
            {
                await _dataService.AddPersonAsync(dialog.Person);
                LoadData();
            }
        }

        private async void AddDeliverable_Click(object sender, RoutedEventArgs e)
        {
            //// have to set a root for the dialog to spawn from
            //var dialog = new AddDeliverableDialog(Accounts.ToList())
            //{
            //    XamlRoot = this.XamlRoot
            //};

            //var result = await dialog.ShowAsync();

            //if (result == ContentDialogResult.Primary && dialog.Deliverable != null)
            //{
            //    await _dataService.AddDeliverableAsync(dialog.Deliverable);
            //    LoadData();
            //}
        }

    }
}
