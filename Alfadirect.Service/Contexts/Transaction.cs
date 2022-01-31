using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alfadirect.Service.Contexts.Entities;

namespace Alfadirect.Service.Contexts
{
    public class Transaction : IDisposable
    {
        private const int _defaultDelay = 2000;
        
        private readonly List<Account> _accounts;
        private readonly List<Account> _transactionAccounts;
        private readonly List<Transfer> _transfers;
        private readonly List<Transfer> _transactionTransfers;

        public Transaction(List<Account> accounts, List<Account> transactionAccounts, 
            List<Transfer> transfers, List<Transfer> transactionTransfers)
        {
            _accounts = accounts;
            _transactionAccounts = transactionAccounts;
            _transfers = transfers;
            _transactionTransfers = transactionTransfers;
        }
        
        
        /// <summary>
        /// Зафиксировать транзакцию
        /// </summary>
        /// <returns></returns>
        public Task CommitAsync()
        {
            _accounts.Clear();
            _accounts.AddRange(_transactionAccounts);
            
            _transfers.Clear();
            _transfers.AddRange(_transactionTransfers);
            
            return Task.Delay(_defaultDelay);
        }

        /// <summary>
        /// Прервать транзакцию
        /// </summary>
        /// <returns></returns>
        public Task AbortAsync()
        {
            ClearTransaction();

            return Task.Delay(_defaultDelay);
        }
        
        public void Dispose()
        {
            ClearTransaction();
        }
        
        private void ClearTransaction()
        {
            _transactionTransfers.Clear();
            _transactionAccounts.Clear();
        }
    }
}