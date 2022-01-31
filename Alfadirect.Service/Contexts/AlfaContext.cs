using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Alfadirect.Service.Contexts.Entities;

namespace Alfadirect.Service.Contexts
{
    public class AlfaContext
    {
        #region Initialization

        private const int _defaultDelay = 2000;

        private List<Customer> _customers = new()
        {
            new Customer
            {
                CustomerId = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                Accounts = new List<string> {"acc1"}
            },
            new Customer
            {
                CustomerId = 2,
                FirstName = "Семен",
                LastName = "Семенов",
                Accounts = new List<string> {"acc2", "acc3"}
            },
            new Customer
            {
                CustomerId = 3,
                FirstName = "Павел",
                LastName = "Павлов",
                Accounts = new List<string> {"acc4", "acc5", "acc6"}
            }
        };

        private List<Account> _accounts = new()
        {
            new Account
            {
                Id = "acc1",
                Balance = 100
            },
            new Account
            {
                Id = "acc2",
                Balance = 0
            },
            new Account
            {
                Id = "acc3",
                Balance = 0
            },
            new Account
            {
                Id = "acc4",
                Balance = 500
            },
            new Account
            {
                Id = "acc5",
                Balance = new decimal(612.92)
            },
            new Account
            {
                Id = "acc6",
                Balance = 9300
            },
        };

        private List<Transfer> _transfers = new()
        {
            new Transfer
            {
                SourceAccountId = "acc5",
                DestinationAccountId = "acc6",
                Amount = 300
            }
        };

        private List<Account> _accountsTransaction;
        private List<Transfer> _transfersTransaction;
        private Transaction _transaction;

        #endregion

        /// <summary>
        /// Список клиентов
        /// </summary>
        public IEnumerable<Customer> Customers => _customers;

        /// <summary>
        /// Список счетов клиентов
        /// </summary>
        public IEnumerable<Account> Accounts => _accounts;

        /// <summary>
        /// Список переводов между счетами клиента
        /// </summary>
        public IEnumerable<Transfer> Transfers => _transfers;

        /// <summary>
        /// Обновить счёт клиента
        /// </summary>
        /// <param name="accountId">Идентификатор счёта</param>
        /// <param name="newBalance">Новый баланс счёта</param>
        public Task UpdateAccountAsync(string accountId, decimal newBalance)
        {
            var data = _transaction is not null ? _accountsTransaction : _accounts;
            
            var account = data.Single(a => a.Id.Equals(accountId));
            var newAccountRecord = new Account {Id = accountId, Balance = newBalance};

            data.Remove(account);
            data.Add(newAccountRecord);

            return Task.Delay(_defaultDelay);
        }

        /// <summary>
        /// Добавить перевод в список трансферов
        /// </summary>
        /// <param name="transfer">Информация о переводе</param>
        public Task AddTransferAsync(Transfer transfer)
        {
            if (_transaction is not null)
            {
                _transfersTransaction.Add(transfer);
            }
            else
            {
                _transfers.Add(transfer);
            }
            
            return Task.Delay(_defaultDelay);
        }
        
        /// <summary>
        /// Начать транзакцию
        /// </summary>
        /// <param name="isolationLevel">Уровень изоляции</param>
        /// <returns>Транзакция</returns>
        public Task<Transaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            _accountsTransaction = new List<Account>(_accounts);
            _transfersTransaction = new List<Transfer>(_transfers);

            _transaction = new Transaction(_accounts, _accountsTransaction, 
                _transfers, _transfersTransaction);
            
            return Task.FromResult(_transaction);
        }
    }
}