using EShopNative.ViewModels;

namespace EShopNative.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(UserRoleEntryViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}