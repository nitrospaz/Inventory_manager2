using System;

namespace inventory_manager.Models
{
    /// <summary>
    /// Represents a financial transaction in the inventory manager.
    /// </summary>
    public class FinanceTransaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public int DebitAccountId { get; set; }
        public int CreditAccountId { get; set; }

        public FinanceAccount DebitAccount { get; set; }
        public FinanceAccount CreditAccount { get; set; }
    }
}
