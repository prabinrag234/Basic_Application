using EShopNative.Interfaces;
using EShopNative.Pages;

namespace EShopNative.Services
{
    public class IdleTimeoutService
    {
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(15);
        private readonly ISessionManager _session;
        private readonly INavigationService _nav;
        private readonly IServiceProvider _services;

        private DateTime _lastActivity;
        private bool _isRunning;

        public IdleTimeoutService(
            ISessionManager session,
            INavigationService nav,
            IServiceProvider services)
        {
            _session = session;
            _nav = nav;
            _services = services;

            ResetTimer();
        }

        public void ResetTimer()
        {
            _lastActivity = DateTime.UtcNow;

            if (!_isRunning)
            {
                _isRunning = true;
                StartMonitoring();
            }
        }

        private async void StartMonitoring()
        {
            while (_isRunning)
            {
                await Task.Delay(10000); // check every 10 seconds

                if (!_session.IsLoggedIn)
                    continue;

                if (DateTime.UtcNow - _lastActivity > _timeout)
                {
                    await _session.ClearSessionAsync();

                    var loginPage = _services.GetRequiredService<UserRoleEntry>();
                    await _nav.SetRootPage(loginPage);

                    _isRunning = false;
                }
            }
        }
    }

}
