using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace _2c2p.Assignment.Data.Model
{
    public class TransactionStatus
    {
        [Column("Values", TypeName = "VARCHAR(8)")]
        public Values V { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Values
        {
            APPROVED,
            REJECTED,
            DONE,
            FAILED,
            FINISHED
        }

        public static EnumToStringConverter<Values> FluentInitAndSeed(ModelBuilder modelBuilder)
        {
            var converter = new EnumToStringConverter<Values>();

            modelBuilder.Entity<TransactionStatus>(entity => {

                entity.HasKey(e => e.V);
                entity.Property(e => e.V).HasConversion(converter);
            });

            var values = Enum.GetValues(typeof(Values)).Cast<Values>();

            foreach (var v in values)
            {
                modelBuilder.Entity<TransactionStatus>(entity =>
                {
                    entity.HasData(new TransactionStatus() { V = v });
                });
            }
            return converter;
        }
    }
}
