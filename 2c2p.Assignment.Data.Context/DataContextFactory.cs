using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace _2c2p.Assignment.Data.Context
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {

        public DataContextFactory()
        {
        }

        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Database=TransactionsDatabase;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.EnableSensitiveDataLogging();

            return new DataContext(optionsBuilder.Options);

        }
    }
}