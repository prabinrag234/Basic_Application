using EShopNative.Helper;
using EShopNative.Interfaces;

namespace EShopNative.Services
{
    public class AlertService : IAlertService
    {
        public Task ShowAlert(string title, string message, string ok = "OK")
        {
            return WindowHelper.GetCurrentPage().DisplayAlertAsync(title, message, ok);
        }

        public Task ShowError(string message, string title = "Error")
        {
            return ShowAlert(title, message);
        }

        public Task ShowSuccess(string message, string title = "Success")
        {
            return ShowAlert(title, message);
        }
    }

}
