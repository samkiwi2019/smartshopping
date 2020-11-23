using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartshopping.Data;
using Microsoft.Extensions.Configuration;

namespace Smartshopping.Installers
{
    public class DbInstaller: IInstaller
    {
        

        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            // to connect mysql 
            services.AddDbContext<MyContext>(opt =>
                opt.UseMySql(configuration.GetConnectionString("connection")));
        }
    }
}