using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace _2c2p.Assignment.Data.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public ISO4217CurrencyCode.Values CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionStatus.Values Status { get; set; }


        public static void FluentInitAndSeed(ModelBuilder modelBuilder, EnumToStringConverter<ISO4217CurrencyCode.Values> iso4217CurrencyCodeConverter, EnumToStringConverter<TransactionStatus.Values> transactionStatusConverter)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.TransactionId) .HasMaxLength(50);
                entity.Property(entity => entity.Amount);
                entity.Property(entity => entity.TransactionDate);
                entity.Property(entity => entity.CurrencyCode).HasConversion(iso4217CurrencyCodeConverter);
                entity.Property(entity => entity.Status).HasConversion(transactionStatusConverter);
            });
        }

    }
}
