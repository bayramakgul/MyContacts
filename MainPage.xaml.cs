using MyContacts.Model;

namespace MyContacts
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        async void AddContact(MContact contact)
        {
            string message = await BL.AddContact(contact);
            if (!string.IsNullOrEmpty(message))
                await DisplayAlert("Hata", message, "Ok");
        }

        async void EditContact(MContact contact)
        {
            string message = await BL.EditContact(contact);
            if (!string.IsNullOrEmpty(message))
                await DisplayAlert("Hata", message, "Ok");
        }

        async void DeleteContact(string id)
        {
            string message = await BL.DeleteContact(id);
            if (!string.IsNullOrEmpty(message))
                await DisplayAlert("Hata", message, "Ok");
        }

        private void AddContactEvent(object sender, EventArgs e)
        {
            ContactEditPage page = new ContactEditPage();
            MContact contact = new MContact();
            page.BindingContext = contact;

            page.AddContact = AddContact;

            Navigation.PushModalAsync(page);
        }

        private void EditContactEvent(object sender, EventArgs e)
        {
            var id = (sender as MenuItem).CommandParameter.ToString();
            ContactEditPage page = new ContactEditPage();
            page.BindingContext = BL.Contacts.First(x => x.Id == id);
            page.EditContact = EditContact;

            Navigation.PushModalAsync(page);

        }

        private async void DeleteContactEvent(object sender, EventArgs e)
        {
            var id = (sender as MenuItem).CommandParameter.ToString();
            bool res = await DisplayAlert("Silmeyi onayla", "Slinsin mi?", "Evet", "Hayır");

            if (res)
                DeleteContact(id);
        }



        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = e.NewTextValue.Trim().ToLower();

            if (string.IsNullOrEmpty(filter))
                listContacts.ItemsSource = BL.Contacts;
            else
            {

                var filteredContacts = BL.Contacts.Where(
                            o=>o.FullName.ToLower().Contains(filter)
                            || o.Phone.ToLower().Contains(filter)
                            || o.Mail.ToLower().Contains(filter));

                listContacts.ItemsSource = filteredContacts;
            }
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
                   
            await Navigation.PushModalAsync(new LoginPage(LoadContacts));
        

        }


        async void LoadContacts()
        {
            var message = await BL.GetContacts();
            listContacts.ItemsSource = BL.Contacts;
            if (!string.IsNullOrEmpty(message))
                await DisplayAlert("Hata", message, "Ok");

        }
    }

}
