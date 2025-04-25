using inventory_manager.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace inventory_manager.Pages.Dialogs
{
    public sealed partial class AddAccountDialog : ContentDialog
    {
        public FinanceAccount Account { get; private set; }

        public AddAccountDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool hasError = false;

            // Validate Account Name
            if (string.IsNullOrWhiteSpace(AccountNameTextBox.Text))
            {
                ShowErrorMessage(AccountNameTextBox, AccountNameErrorTextBlock, "Account name cannot be empty.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(AccountNameTextBox, AccountNameErrorTextBlock);
            }

            if (hasError)
            {
                args.Cancel = true;
                return;
            }

            // Create the account if all validations pass
            Account = new FinanceAccount { Name = AccountNameTextBox.Text };
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
            Account = null;
        }

        private void AccountNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AccountNameTextBox.Text))
            {
                ShowErrorMessage(AccountNameTextBox, AccountNameErrorTextBlock, "Account name cannot be empty.");
            }
            else
            {
                ClearErrorMessage(AccountNameTextBox, AccountNameErrorTextBlock);
            }
        }
    }
}