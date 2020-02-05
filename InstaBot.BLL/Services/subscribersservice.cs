using System;
using System.Threading.Tasks;
using InstaBot.BLL.Services.Abstract;
using InstaSharper.API;
using InstaSharper.Classes;

namespace InstaBot.BLL.Services
{
    public class SubscribersService : ISubscribersService
    {
        private readonly IInstaApi _api;

        public SubscribersService(IInstaApi api)
        {
            _api = api;
        }

        public async Task<int> RemoveSubscribersAsync(int count)
        {
            if (!_api.IsUserAuthenticated)
                await _api.LoginAsync();

            var type = _api.GetType();
            type.GetProperty("IsUserAuthenticated")?.SetValue(_api, true);

            var currentUser = await _api.GetCurrentUserAsync();
            var followings = await _api.GetUserFollowingAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(20));
            var iteration = 0;

            foreach (var following in followings.Value)
            {
                if (iteration == count)
                    return iteration;

                await Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(20,200)));
                var unFollowResult = await _api.UnFollowUserAsync(following.Pk);

                Console.WriteLine($"{(unFollowResult.Succeeded ? $"Success unfollowed {following.UserName}" : unFollowResult.Info.Message)}");

                iteration++;
            }

            return iteration;
        }
    }
}