namespace EShopNative.Interfaces
{
    public interface IAlertService
    {
        Task ShowAlert(string title, string message, string ok = "OK");
        Task ShowError(string message, string title = "Error");
        Task ShowSuccess(string message, string title = "Success");
    }

}
