using EnglishLearningProject.Models;
using Microsoft.AspNetCore.Identity;

namespace EnglishLearningProject.Extensions
{
    public static class StartUpExtensions
    {
        public static void CustomAddIdentityWithExtensions(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(1);
            });


            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuwxyz1234567890_";
                options.Password.RequiredLength = 8; //şifre uzunluk kısıtlaması
                options.Password.RequireLowercase = true; //küçük harf kısıtlaması
                options.Password.RequireUppercase = true; //büyük harf kısıtlaması
                options.Password.RequireDigit = true; //sayısal ifadeler kısıtlaması

                //LockoutOnFailure (Kilit Mekanizmasi)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddDefaultTokenProviders()
              .AddEntityFrameworkStores<AppDbContext>();

        }

    }
}
