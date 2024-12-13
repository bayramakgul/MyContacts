using MyContacts.Model;

namespace MyContacts;

public partial class ContactEditPage : ContentPage
{
    public Action<MContact> AddContact;
    public Action<MContact> EditContact;

    public MContact Contact;

	public ContactEditPage()
	{
		InitializeComponent();

        Contact = this.BindingContext as MContact;
	}

    private async void AddImageClicked(object sender, EventArgs e)
    {
        // Kullanýcýya seçim ekranýný göster
        string action = await DisplayActionSheet(
            "Resim Ekle",
            "Ýptal",
            null,
            "Kamera",
            "Galeri");

        try
        {
            FileResult photo = null;

            if (action == "Kamera")
            {
                // Kameradan fotoðraf çek
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    photo = await MediaPicker.Default.CapturePhotoAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Hata", "Kamera desteklenmiyor.", "Tamam");
                }
            }
            else if (action == "Galeri")
            {
                // Galeriden fotoðraf seç
                photo = await MediaPicker.Default.PickPhotoAsync();
            }

            if (photo != null)
            {
                Contact = this.BindingContext as MContact;
                // Fotoðraf dosyasýný al
                Contact.Image = photo.FullPath;
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
        }

    }

    private void DeleteImageClicked(object sender, EventArgs e)
    {
        Contact = this.BindingContext as MContact;
        // Fotoðraf dosyasýný al
        Contact.Image = null;
    }

    private void OkClicked(object sender, EventArgs e)
    {
        Contact = this.BindingContext as MContact;

        AddContact?.Invoke(Contact);
        EditContact?.Invoke(Contact);

        Navigation.PopModalAsync();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();

    }
}