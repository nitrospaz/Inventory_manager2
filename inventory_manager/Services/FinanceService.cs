using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using inventory_manager.Models;

namespace inventory_manager.Services
{
    class FinanceService
    {
        public FinanceService()
        {
            using (var db = new FinanceContext())
            {
                db.EnsureDatabaseCreated();
            }
        }

        public async Task AddFinanceTransactionAsync(FinanceTransaction transaction)
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    using (var dbTransaction = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Add the new transaction
                            await db.FinanceTransactions.AddAsync(transaction);
                            await db.SaveChangesAsync();

                            // Find the corresponding accounts
                            var debitAccount = await db.FinanceAccounts.FindAsync(transaction.DebitAccountId);
                            var creditAccount = await db.FinanceAccounts.FindAsync(transaction.CreditAccountId);

                            if (debitAccount == null)
                            {
                                throw new Exception("Debit account not found.");
                            }

                            if (creditAccount == null)
                            {
                                throw new Exception("Credit account not found.");
                            }

                            // Update the balances
                            debitAccount.Balance -= transaction.Amount;
                            creditAccount.Balance += transaction.Amount;

                            db.FinanceAccounts.Update(debitAccount);
                            db.FinanceAccounts.Update(creditAccount);

                            await db.SaveChangesAsync();

                            // Create an audit log entry
                            var auditLog = new AuditLog
                            {
                                Timestamp = DateTime.UtcNow,
                                Action = "AddFinanceTransaction",
                                Details = $"TransactionId: {transaction.Id}, Amount: {transaction.Amount}, DebitAccountId: {transaction.DebitAccountId}, CreditAccountId: {transaction.CreditAccountId}"
                            };

                            await db.AuditLogs.AddAsync(auditLog);
                            await db.SaveChangesAsync();

                            // Commit the transaction
                            await dbTransaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction if any error occurs
                            await dbTransaction.RollbackAsync();
                            Debug.WriteLine($"Error committing transaction: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding finance transaction: {ex.Message}");
            }
        }

        public async Task UpdateFinanceTransactionAsync(FinanceTransaction transaction)
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    var existingTransaction = await db.FinanceTransactions.FindAsync(transaction.Id);
                    if (existingTransaction != null)
                    {
                        existingTransaction.Description = transaction.Description;
                        existingTransaction.Amount = transaction.Amount;
                        existingTransaction.Date = transaction.Date;

                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating finance transaction: {ex.Message}");
            }
        }

        public List<FinanceTransaction> GetFinanceTransactions()
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    return db.FinanceTransactions.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting finance transactions: {ex.Message}");
                return new List<FinanceTransaction>();
            }
        }

        public async Task DeleteFinanceTransactionAsync(int id)
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    var transaction = await db.FinanceTransactions.FindAsync(id);
                    if (transaction != null)
                    {
                        db.FinanceTransactions.Remove(transaction);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting finance transaction: {ex.Message}");
            }
        }

        public List<FinanceAccount> GetFinanceAccounts()
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    return db.FinanceAccounts.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting finance accounts: {ex.Message}");
                return new List<FinanceAccount>();
            }
        }

        public async Task AddFinanceAccountAsync(FinanceAccount account)
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    await db.FinanceAccounts.AddAsync(account);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding finance account: {ex.Message}");
            }
        }

        public async Task UpdateFinanceAccountAsync(FinanceAccount account)
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    var existingAccount = await db.FinanceAccounts.FindAsync(account.Id);
                    if (existingAccount != null)
                    {
                        existingAccount.Name = account.Name;
                        existingAccount.Balance = account.Balance;

                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating finance account: {ex.Message}");
            }
        }

        public async Task DeleteFinanceAccountAsync(int id)
        {
            try
            {
                using (var db = new FinanceContext())
                {
                    var account = await db.FinanceAccounts.FindAsync(id);
                    if (account != null)
                    {
                        db.FinanceAccounts.Remove(account);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting finance account: {ex.Message}");
            }
        }
    }
}