#define FIREBASE

using System.Data;
using System.Collections.ObjectModel;


#if MYSQL
using MySql.Data.MySqlClient;
using DbConnectionStringBuilder = MySql.Data.MySqlClient.MySqlConnectionStringBuilder;
using DbConnection = MySql.Data.MySqlClient.MySqlConnection;
using DbCommand = MySql.Data.MySqlClient.MySqlCommand;
using DbDataReader = MySql.Data.MySqlClient.MySqlDataReader;

#elif MSSQL

#elif FIREBASE
using Firebase.Database;
using Firebase.Database.Query;
#endif



namespace MyContacts.Model
{
    public static class DL
    {
#if FIREBASE

        static FirebaseClient client = new FirebaseClient("https://mycontacts-548d1-default-rtdb.firebaseio.com/");
        public static async Task<(ObservableCollection<MContact> list, string message)> GetContacts()
        {
            ObservableCollection<MContact> list = new ObservableCollection<MContact>();

            try
            {
                var contacts = (await client.Child("my_contacts").
                    OnceAsync<MContact>()).
                    Select(item=> new MContact()
                    {
                        Id = item.Object.Id,
                        Name = item.Object.Name,
                        Surname = item.Object.Surname,
                        Phone = item.Object.Phone,
                        Mail = item.Object.Mail,
                        ImageData = item.Object.ImageData,

                    }).ToList();


                foreach (var contact in contacts)
                {
                    list.Add(contact);
                }

                return (list, "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public static async Task<(bool isSuccess, string message)> AddContact(MContact contact)
        {
            try
            {
                await client.Child($"my_contacts/{contact.Id}").PutAsync(contact);
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public static async Task<(bool isSuccess, string message)> UpdateContact(MContact contact)
        {
            try
            {
                await client.Child($"my_contacts/{contact.Id}").PutAsync(contact);
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public static async Task<(bool isSuccess, string message)> DeleteContact(string id)
        {
            try
            {
                await client.Child($"my_contacts/{id}").DeleteAsync();
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }


#else
        static DbConnection conn = new DbConnection(
            new DbConnectionStringBuilder()
            {
                Server = "172.24.144.1", //"localhost",
                Database = "my_contacts_db",
                UserID = "my_user",
                Password = "my_password123",
                Port = 3306
            }.ConnectionString);

        public static async Task<(ObservableCollection<MContact> list, string message)> GetContacts() 
        {
            ObservableCollection<MContact> list = new ObservableCollection<MContact>();

            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                DbCommand cmd = new DbCommand("SELECT * FROM my_contacts", conn);
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new MContact()
                    {
                        Id      = reader["c_id"].ToString(),
                        Name    = reader.GetString("c_name"),
                        Surname = reader.GetString("c_sname"),
                        Phone   = reader.GetString("c_phone"),
                        Mail    = reader.GetString("c_mail"),
                        ImageData = !reader.IsDBNull("c_image") ? reader.GetString("c_image") : null,

                    });
                }

            }
            catch (Exception ex)
            {
                 return (null, ex.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            return (list, "");
        }

        public static async Task<(bool isSuccess, string message)> AddContact(MContact contact)
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                DbCommand cmd = new DbCommand("INSERT INTO my_contacts" +
                    " VALUES (@id, @nm, @sn, @pn, @ml, @im)", conn);

                cmd.Parameters.AddWithValue("@id", contact.Id);
                cmd.Parameters.AddWithValue("@nm", contact.Name);
                cmd.Parameters.AddWithValue("@sn", contact.Surname);
                cmd.Parameters.AddWithValue("@pn", contact.Phone);
                cmd.Parameters.AddWithValue("@ml", contact.Mail);
                cmd.Parameters.AddWithValue("@im", contact.ImageData);

                cmd.ExecuteNonQuery();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

        }

        public static async Task<(bool isSuccess, string message)> UpdateContact(MContact contact)
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                DbCommand cmd = new DbCommand(
                    "UPDATE my_contacts " +
                    " SET c_name=@nm," +
                    "     c_sname=@sn," +
                    "     c_phone=@pn," +
                    "     c_mail =@ml," +
                    "     c_image=@im " +
                    "WHERE " +
                    "     c_id = @id", conn);

                cmd.Parameters.AddWithValue("@nm", contact.Name);
                cmd.Parameters.AddWithValue("@sn", contact.Surname);
                cmd.Parameters.AddWithValue("@pn", contact.Phone);
                cmd.Parameters.AddWithValue("@ml", contact.Mail);
                cmd.Parameters.AddWithValue("@im", contact.ImageData);
                cmd.Parameters.AddWithValue("@id", contact.Id);

                cmd.ExecuteNonQuery();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

        }

        public static async Task<(bool isSuccess, string message)> DeleteContact(string id)
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                DbCommand cmd = new DbCommand(
                    "DELETE FROM my_contacts " +
                    "WHERE c_id = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

        }
#endif
    }
}
