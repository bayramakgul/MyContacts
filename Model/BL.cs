using Firebase.Auth.Providers;
using Firebase.Auth;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZstdSharp.Unsafe;
using Microsoft.Maui.ApplicationModel.Communication;

namespace MyContacts.Model
{
    public static class BL
    {
        public static ObservableCollection<MContact> Contacts { get; set; }

        public static async Task<string> GetContacts()
        {
            var res = await DL.GetContacts();
            Contacts = res.list;
            return res.message;
        }

        public static async Task<string> AddContact(MContact contact)
        {
            var res = await DL.AddContact(contact);
            if (res.isSuccess)
                Contacts.Add(contact);

            return res.message;
        }

        public static async Task<string> EditContact(MContact contact)
        {
            var res = await DL.UpdateContact(contact);
            //if (res.isSuccess)
            //    Contacts.Add(contact);

            return res.message;
        }

        public static async Task<string> DeleteContact(string id)
        {
            var res = await DL.DeleteContact(id);
            if (res.isSuccess)
            {
                var c = Contacts.First(o=>o.Id == id);
                Contacts.Remove(c);
            }

            return res.message;
        }

    }

    public static class   FirebaseServices
    {
        static string projectid = "mycontacts-548d1";
        static FirebaseAuthConfig config = new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyD4Zu0BLl-fRNtllS-gss2ruQO5H14-Ulw",
            AuthDomain = $"{projectid}.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]{ new EmailProvider()},
        };


        public async static Task<bool> Login(string mail, string pass)
        {
            try
            {
                var client = new FirebaseAuthClient(config);
                var res = await client.SignInWithEmailAndPasswordAsync(mail, pass);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async static Task<bool> Register(string name, string mail, string pass)
        {
            try
            {
                var client = new FirebaseAuthClient(config);
                var res = await client.CreateUserWithEmailAndPasswordAsync(mail, pass, name);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
