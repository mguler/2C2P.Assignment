using _2c2p.Assignment.Bussiness;
using _2c2p.Assignment.Data.Context;
using _2c2p.Assignment.Model.Mapping;
using _2c2p.Assignment.Tools.Impl.Mapping;
using _2c2p.Assignment.Tools.Impl.Validation;
using _2c2p.Assignment.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _2c2p.Assignment.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMappingService(new XmlToTransactionMappingConfiguration()
                , new CsvToTransactionMappingConfiguration()
                , new TransactionToGetAllTransactionsResponseModelMappingConfiguration());
            services.AddValidationService(new TransactionCsvValidationConfiguration(), new TransactionXmlValidationConfiguration());
            services.AddTransient<TransactionManager>();
            services.AddTransient<DataContext>();
            services.AddDbContext<DataContext>(options => options.UseSqlServer("Data Source=.\\SQLEXPRESS;Database=TransactionsDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"), ServiceLifetime.Transient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Transaction}/{action=Upload}");

                endpoints.MapControllerRoute(
                    name: "get all transactions by currency",
                    pattern: "{controller=Transaction}/{action=GetAllTransactionsByCurrency}");

                endpoints.MapControllerRoute(
                    name: "get all transactions by date range",
                    pattern: "{controller=Transaction}/{action=GetAllTransactionsByDateRange}");

                endpoints.MapControllerRoute(
                    name: "get all transactions by status",
                    pattern: "{controller=Transaction}/{action=GetAllTransactionsByStatus}");

            });
        }
    }
}
