using inventory_manager.Models;
using inventory_manager.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
        private List<Person> _persons;

        public ProjectConfiguration()
        {
            this.InitializeComponent();
            _dataService = new ProjectDataService(new InventoryContext());
            LoadPersonsAsync();
        }

        private async Task LoadPersonsAsync()
        {
            _persons = await _dataService.GetAllPeopleAsync();
            PersonsListView.ItemsSource = _persons;
        }

        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var person = new Person(
                NameTextBox.Text,
                (PersonRole)RoleComboBox.SelectedIndex,
                EmailTextBox.Text
            );

            await _dataService.AddPersonAsync(person);
            await LoadPersonsAsync();
            ClearForm();
        }

        private async void UpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            if (PersonsListView.SelectedItem is Person selectedPerson)
            {
                selectedPerson.Name = NameTextBox.Text;
                selectedPerson.Email = EmailTextBox.Text;
                selectedPerson.Role = (PersonRole)RoleComboBox.SelectedIndex;

                await _dataService.UpdatePersonAsync(selectedPerson);
                await LoadPersonsAsync();
                ClearForm();
            }
        }

        private async void DeletePerson_Click(object sender, RoutedEventArgs e)
        {
            if (PersonsListView.SelectedItem is Person selectedPerson)
            {
                await _dataService.DeletePersonAsync(selectedPerson.Id);
                await LoadPersonsAsync();
                ClearForm();
            }
        }

        private void PersonsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonsListView.SelectedItem is Person selectedPerson)
            {
                NameTextBox.Text = selectedPerson.Name;
                EmailTextBox.Text = selectedPerson.Email;
                RoleComboBox.SelectedIndex = (int)selectedPerson.Role;
            }
        }

        private void ClearForm()
        {
            NameTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            RoleComboBox.SelectedIndex = -1;
            PersonsListView.SelectedItem = null;
        }
    }
}
