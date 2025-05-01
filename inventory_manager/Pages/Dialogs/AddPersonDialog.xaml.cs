using inventory_manager.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace inventory_manager.Pages.Dialogs
{
    public sealed partial class AddPersonDialog : ContentDialog
    {
        public Person Person { get; private set; }

        public AddPersonDialog()
        {
            this.InitializeComponent();
            PopulateRoleComboBox();
        }

        private void PopulateRoleComboBox()
        {
            RoleComboBox.ItemsSource = Enum.GetValues(typeof(PersonRole));
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool hasError = false;

            // Validate Name
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowErrorMessage(NameTextBox, NameErrorTextBlock, "Name cannot be empty.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(NameTextBox, NameErrorTextBlock);
            }

            // Validate Email
            //if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            //{
            //    ShowErrorMessage(EmailTextBox, EmailErrorTextBlock, "Email cannot be empty.");
            //    hasError = true;
            //}
            //else
            //{
            //    ClearErrorMessage(EmailTextBox, EmailErrorTextBlock);
            //}

            // Validate Role
            if (RoleComboBox.SelectedItem == null)
            {
                ShowErrorMessage(RoleComboBox, RoleErrorTextBlock, "A valid role must be selected.");
                hasError = true;
            }
            else
            {
                // check if it is a valid role
                if (!(RoleComboBox.SelectedItem is PersonRole))
                {
                    ShowErrorMessage(RoleComboBox, RoleErrorTextBlock, "A valid role must be selected.");
                    hasError = true;
                }
                else
                {
                    ClearErrorMessage(RoleComboBox, RoleErrorTextBlock);
                }
            }

            // Check if there are any errors
            if (hasError)
            {
                args.Cancel = true;
                return;
            }

            // Create the account if all validations pass
            Person = new Person(NameTextBox.Text, (PersonRole)Enum.Parse(typeof(PersonRole), RoleComboBox.SelectedItem.ToString()), string.Empty);
        }

        private void ShowErrorMessage(Control control, TextBlock errorTextBlock, string message)
        {
            control.BorderBrush = new SolidColorBrush(Colors.Red);
            errorTextBlock.Text = message;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ClearErrorMessage(Control control, TextBlock errorTextBlock)
        {
            control.BorderBrush = new SolidColorBrush(Colors.Transparent);
            errorTextBlock.Text = string.Empty;
            errorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Person = null;
        }
    }
}