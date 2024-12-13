using MyContacts.Model;

namespace MyContacts;

public partial class LoginPage : ContentPage
{
	public LoginPage(Action action)
	{
		InitializeComponent();

        LoadContacts = action;
	}

    Action LoadContacts;

    private void Reg_Clicked(object sender, EventArgs e)
    {
        if (loginStack.IsVisible)
        {
            loginStack.IsVisible = false;
            registerStack.IsVisible = true;
        }
        else
        {
            loginStack.IsVisible = true;
            registerStack.IsVisible = false;

        }
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        var res = await FirebaseServices.Login(lEmail.Text, lPassword.Text);
        if (res)
        {
            await Navigation.PopModalAsync();
            LoadContacts.Invoke();
        }
        else
            await DisplayAlert("Hata", "Kullanýcý adý veya þifre hatalý!", "OK");
    }  
    
    private async void Register_Clicked(object sender, EventArgs e)
    {
        var res = await FirebaseServices.Register(rDispName.Text, rEmail.Text, rPassword.Text);
        if (res)
            await Navigation.PopModalAsync();
        else
            await DisplayAlert("Hata", "Kayýt baþarýsýz!", "OK");
    }
}