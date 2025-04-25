using inventory_manager.Models;
using inventory_manager.Pages.Dialogs;
using inventory_manager.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace inventory_manager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinancePage : Page
    {
        private FinanceService _dataAccess;
        ObservableCollection<FinanceAccount> Accounts { get; set; }
        ObservableCollection<FinanceTransaction> Transactions { get; set; }

        public FinancePage()
        {
            this.InitializeComponent();
            _dataAccess = new FinanceService();
            Accounts = new ObservableCollection<FinanceAccount>();
            Transactions = new ObservableCollection<FinanceTransaction>();

            // Bind the collections to the ListViews
            AccountsListView.ItemsSource = Accounts;
            TransactionsListView.ItemsSource = Transactions;

            // Load data
            LoadData();
        }

        private void LoadData()
        {
            var accounts = _dataAccess.GetFinanceAccounts();
            var transactions = _dataAccess.GetFinanceTransactions();

            // Clear existing items
            Accounts.Clear();
            Transactions.Clear();

            // Add new items
            foreach (var account in accounts)
            {
                Accounts.Add(account);
            }

            foreach (var transaction in transactions)
            {
                transaction.DebitAccount = Accounts.FirstOrDefault(a => a.Id == transaction.DebitAccountId);
                transaction.CreditAccount = Accounts.FirstOrDefault(a => a.Id == transaction.CreditAccountId);
                Transactions.Add(transaction);
            }
        }

        private async void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            // have to set a root for the dialog to spawn from
            var dialog = new AddTransactionDialog(Accounts.ToList())
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && dialog.Transaction != null)
            {
                await _dataAccess.AddFinanceTransactionAsync(dialog.Transaction);
                LoadData();
            }
        }

        private async void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddAccountDialog
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && dialog.Account != null)
            {
                await _dataAccess.AddFinanceAccountAsync(dialog.Account);
                LoadData();
            }
        }
    }
}
