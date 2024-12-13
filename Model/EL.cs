using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyContacts.Model
{
    public class MContact : INotifyPropertyChanged
    {
        private string id, name, sname, mail, phone, image;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Id
        {
            get {
                if(string.IsNullOrEmpty(id))
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            
            set
            {
                name = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullName");
            }
        }

        public string Surname
        {
            get => sname;
            set
            {
                sname = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullName");
            }
        }

        [JsonIgnore]
        public string FullName => name + " " + sname;

        public string Phone
        {
            get => phone;
            
            set
            {
                phone = value;
                NotifyPropertyChanged();
            }
        }


        public string Mail
        {
            get => mail;
            set
            {
                mail = value;
                NotifyPropertyChanged();
            }
        }

        public string Image
        {
            get => image; 
            set
            {
                image = value;
                NotifyPropertyChanged();
            }
        }

        public string ImageData
        {
            get
            {
                if (string.IsNullOrEmpty(image))
                    return null;
                else
                {
                    var imdata = File.ReadAllBytes(image);
                    return Convert.ToBase64String(imdata);
                }
            }
            set
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, id);
                var imdata = Convert.FromBase64String(value);
                File.WriteAllBytes(path, imdata);

                image = path;
                NotifyPropertyChanged("Image");
            }
        }
    }
}
