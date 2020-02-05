using System.Threading.Tasks;
using InstaBot.BLL.Services;
using InstaBot.BLL.Services.Abstract;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace InstaBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var userName = "+380505994293";
            var password = "nalugo59";
            var services = CreateServices(userName, password);

            using (var scope = services.BuildServiceProvider(false).CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var subscribeService = serviceProvider.GetRequiredService<ISubscribersService>();

                await subscribeService.RemoveSubscribersAsync(100);
            }
        }

        private static IServiceCollection CreateServices(string userName, string password)
            => new ServiceCollection()
                .AddSingleton(BuildInstaApi(userName, password))
                .AddSingleton<ISubscribersService, SubscribersService>();

        private static IInstaApi BuildInstaApi(string userName, string password)
            => InstaApiBuilder
                .CreateBuilder()
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetUser(new UserSessionData
                {
                    UserName = userName,
                    Password = password
                })
                .Build();
    }
}
