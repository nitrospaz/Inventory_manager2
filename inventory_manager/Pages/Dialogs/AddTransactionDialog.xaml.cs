using inventory_manager.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;

namespace inventory_manager.Pages.Dialogs
{
    public sealed partial class AddTransactionDialog : ContentDialog
    {
        public FinanceTransaction Transaction { get; private set; }
        public List<FinanceAccount> Accounts { get; set; }

        public AddTransactionDialog(List<FinanceAccount> accounts)
        {
            this.InitializeComponent();
            Accounts = accounts;
            DebitAccountComboBox.ItemsSource = Accounts;
            CreditAccountComboBox.ItemsSource = Accounts;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool hasError = false;

            // Validate Description
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                ShowErrorMessage(DescriptionTextBox, DescriptionErrorTextBlock, "Description cannot be empty.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(DescriptionTextBox, DescriptionErrorTextBlock);
            }

            // Validate Amount
            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                ShowErrorMessage(AmountTextBox, AmountErrorTextBlock, "Please enter a valid amount.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(AmountTextBox, AmountErrorTextBlock);
            }

            // Validate Date
            if (DatePicker.Date == null)
            {
                ShowErrorMessage(DatePicker, DateErrorTextBlock, "Please select a valid date.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(DatePicker, DateErrorTextBlock);
            }

            // Validate Debit Account
            if (DebitAccountComboBox.SelectedItem == null)
            {
                ShowErrorMessage(DebitAccountComboBox, DebitAccountErrorTextBlock, "Please select a valid debit account.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(DebitAccountComboBox, DebitAccountErrorTextBlock);
            }

            // Validate Credit Account
            if (CreditAccountComboBox.SelectedItem == null)
            {
                ShowErrorMessage(CreditAccountComboBox, CreditAccountErrorTextBlock, "Please select a valid credit account.");
                hasError = true;
            }
            else
            {
                ClearErrorMessage(CreditAccountComboBox, CreditAccountErrorTextBlock);
            }

            if (hasError)
            {
                args.Cancel = true;
                return;
            }

            // Create the transaction if all validations pass
            Transaction = new FinanceTransaction
            {
                Description = DescriptionTextBox.Text,
                Amount = amount,
                Date = DatePicker.Date.DateTime,
                DebitAccountId = ((FinanceAccount)DebitAccountComboBox.SelectedItem).Id,
                CreditAccountId = ((FinanceAccount)CreditAccountComboBox.SelectedItem).Id
            };
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
            Transaction = null;
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                ShowErrorMessage(DescriptionTextBox, DescriptionErrorTextBlock, "Description cannot be empty.");
            }
            else
            {
                ClearErrorMessage(DescriptionTextBox, DescriptionErrorTextBlock);
            }
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!decimal.TryParse(AmountTextBox.Text, out _))
            {
                ShowErrorMessage(AmountTextBox, AmountErrorTextBlock, "Please enter a valid amount.");
            }
            else
            {
                ClearErrorMessage(AmountTextBox, AmountErrorTextBlock);
            }
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs args)
        {
            if (DatePicker.Date == null)
            {
                ShowErrorMessage(DatePicker, DateErrorTextBlock, "Please select a valid date.");
            }
            else
            {
                ClearErrorMessage(DatePicker, DateErrorTextBlock);
            }
        }

        private void DebitAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DebitAccountComboBox.SelectedItem == null)
            {
                ShowErrorMessage(DebitAccountComboBox, DebitAccountErrorTextBlock, "Please select a valid debit account.");
            }
            else
            {
                ClearErrorMessage(DebitAccountComboBox, DebitAccountErrorTextBlock);
            }
        }

        private void CreditAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CreditAccountComboBox.SelectedItem == null)
            {
                ShowErrorMessage(CreditAccountComboBox, CreditAccountErrorTextBlock, "Please select a valid credit account.");
            }
            else
            {
                ClearErrorMessage(CreditAccountComboBox, CreditAccountErrorTextBlock);
            }
        }
    }
}
