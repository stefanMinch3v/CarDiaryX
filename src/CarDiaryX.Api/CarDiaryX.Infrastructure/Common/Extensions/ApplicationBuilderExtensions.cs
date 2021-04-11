using CarDiaryX.Infrastructure.Common.Persistence;
using CarDiaryX.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private const string ADMINISTRATOR = "Administrator";

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using (var services = app.ApplicationServices.CreateScope())
            {
                var dbContext = services.ServiceProvider.GetService<CarDiaryXDbContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder AddDefaultUser(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope
                    .ServiceProvider
                    .GetService<RoleManager<IdentityRole>>();

                var userManager = serviceScope
                    .ServiceProvider
                    .GetService<UserManager<User>>();

                Task.Run(async () =>
                {
                    var roles = new[] { ADMINISTRATOR };

                    foreach (var role in roles)
                    {
                        var existingRole = await roleManager.RoleExistsAsync(role);

                        if (!existingRole)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }
                    }

                    var admin = "admin"; 
                    var adminEmail = "admin@mymail.com";
                    var adminFromDb = await userManager.FindByEmailAsync(adminEmail);

                    if (adminFromDb is null)
                    {
                        var user = new User(adminEmail, "000000")
                        {
                            UserName = adminEmail,
                            FirstName = admin,
                            LastName = admin
                        };

                        await userManager.CreateAsync(user, "admin12");

                        await userManager.AddToRoleAsync(user, ADMINISTRATOR);
                    }
                })
                .GetAwaiter()
                .GetResult();
            }

            return app;
        }
    }
}
