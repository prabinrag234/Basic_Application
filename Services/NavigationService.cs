using EShopNative.Helper;
using EShopNative.Interfaces;

namespace EShopNative.Services
{
    public class NavigationService : INavigationService
    {
        public Task PushAsync(Page page)
        {
            var idle = ServiceHelper.GetService<IdleTimeoutService>();
            idle.ResetTimer();
            return WindowHelper.GetCurrentPage().Navigation.PushAsync(page);
        }

        public Task PopAsync()
        {
            return WindowHelper.GetCurrentPage().Navigation.PopAsync();
        }

        public Task PopToRootAsync()
        {
            return WindowHelper.GetCurrentPage().Navigation.PopToRootAsync();
        }

        public Task PushModalAsync(Page page)
        {
            return WindowHelper.GetCurrentWindow().Page.Navigation.PushModalAsync(page);
        }

        public Task PopModalAsync()
        {
            return WindowHelper.GetCurrentWindow().Page.Navigation.PopModalAsync();
        }

        public Task SetRootPage(Page page)
        {
            WindowHelper.GetCurrentWindow().Page = new NavigationPage(page);
            return Task.CompletedTask;
        }
    }

}
