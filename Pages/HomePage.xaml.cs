using EShopNative.ViewModels;

namespace EShopNative.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
    protected override bool OnBackButtonPressed()
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif

        return true; // prevent navigation
    }

}