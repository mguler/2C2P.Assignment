using _2c2p.Assignment.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace _2c2p.Assignment.Data.Context
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Transaction> Transactions { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var transactionStatusConverter = TransactionStatus.FluentInitAndSeed(modelBuilder);
            var iso4217CurrencyCode = ISO4217CurrencyCode.FluentInitAndSeed(modelBuilder);

            Transaction.FluentInitAndSeed(modelBuilder, iso4217CurrencyCode, transactionStatusConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
