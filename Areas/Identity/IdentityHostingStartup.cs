using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Areas.Identity.Data;
using TaskManagement.Data;

[assembly: HostingStartup(typeof(TaskManagement.Areas.Identity.IdentityHostingStartup))]
namespace TaskManagement.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDbContext<AuthDbContext>(options =>
                   options.UseNpgsql(
                       context.Configuration.GetConnectionString("AuthDbContextConnection"))
            );


                services.AddDefaultIdentity<ApplicationUser>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<AuthDbContext>();
            });
        }
    }
}








