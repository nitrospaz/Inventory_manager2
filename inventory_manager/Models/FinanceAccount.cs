using System.Collections.Concurrent;
using System.Collections.Generic;

namespace inventory_manager.Models
{
    /// <summary>
    /// Represents a financial account in the inventory manager.
    /// </summary>
    public class FinanceAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }


        // Properties
        // if not multithreaded you can use ICollection
        //public ICollection<FinanceTransaction> Transactions { get; set; } = new List<FinanceTransaction>();

        // if multithreaded you can use ConcurrentBag

        // Separate navigation properties for debit and credit transactions
        //public ConcurrentBag<FinanceTransaction> DebitTransactions { get; set; } = new ConcurrentBag<FinanceTransaction>();
        //public ConcurrentBag<FinanceTransaction> CreditTransactions { get; set; } = new ConcurrentBag<FinanceTransaction>();

        public ICollection<FinanceTransaction> DebitTransactions { get; set; } = new List<FinanceTransaction>();
        public ICollection<FinanceTransaction> CreditTransactions { get; set; } = new List<FinanceTransaction>();
    }
}
