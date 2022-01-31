using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Alfadirect.Service.Contexts;
using Alfadirect.Service.Contexts.Entities;
using NUnit.Framework;

namespace AlfaDirect.Tests
{
    [TestFixture(TestOf = typeof(AlfaContext), Category = "Unit")]
    public class AlfaContextTests
    {
        [Test]
        public async Task UpdateAccountAsync_WithoutCommitTransaction()
        {
            var context = new AlfaContext();

            var accountId = "acc1";
            var account = context.Accounts.Single(a => a.Id.Equals(accountId));
            var oldBalance = account.Balance;

            using var transaction = await context.BeginTransactionAsync(IsolationLevel.Serializable);
            await context.UpdateAccountAsync(accountId, 5000);

            Assert.AreEqual(oldBalance, context.Accounts.Single(a => a.Id.Equals(accountId)).Balance);
        }
        
        [Test]
        public async Task UpdateAccountAsync_CommitTransaction()
        {
            var context = new AlfaContext();
            
            var accountId = "acc1";
            var account = context.Accounts.Single(a => a.Id.Equals(accountId));
            var oldBalance = account.Balance;
            
            using var transaction = await context.BeginTransactionAsync(IsolationLevel.Serializable);
            await context.UpdateAccountAsync(accountId, 5000);
            await transaction.CommitAsync();
            
            Assert.AreNotEqual(oldBalance, context.Accounts.Single(a => a.Id.Equals(accountId)).Balance);
        }

        [Test]
        public async Task AddTransferAsync_WithoutCommitTransaction()
        {
            var context = new AlfaContext();

            var transferCount = context.Transfers.Count();

            await context.BeginTransactionAsync(IsolationLevel.Serializable);
            await context.AddTransferAsync(new Transfer
                {SourceAccountId = "acc6", DestinationAccountId = "acc5", Amount = 100});
            
            Assert.AreEqual(transferCount, context.Transfers.Count());
        }
        
        [Test]
        public async Task AddTransferAsync_CommitTransaction()
        {
            var context = new AlfaContext();
            
            var transferCount = context.Transfers.Count();
            
            using var transaction = await context.BeginTransactionAsync(IsolationLevel.Serializable);
            await context.AddTransferAsync(new Transfer
                {SourceAccountId = "acc6", DestinationAccountId = "acc5", Amount = 100});
            await transaction.CommitAsync();
            
            Assert.AreNotEqual(transferCount, context.Transfers.Count());
        }
        
        [Test]
        public async Task AddTransferAsync_AbortTransaction()
        {
            var context = new AlfaContext();
            
            var transferCount = context.Transfers.Count();
            
            using var transaction = await context.BeginTransactionAsync(IsolationLevel.Serializable);
            await context.AddTransferAsync(new Transfer
                {SourceAccountId = "acc6", DestinationAccountId = "acc5", Amount = 100});
            await transaction.AbortAsync();
            
            Assert.AreEqual(transferCount, context.Transfers.Count());
        }
    }
}