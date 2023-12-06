using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Product.Web.Areas.Identity.IdentityHostingStartup))]
namespace Product.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}