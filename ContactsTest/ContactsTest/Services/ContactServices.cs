using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;

using SQLite;

using Xamarin.Essentials;

namespace ContactsTest.Services
{
    internal class ContactServices : IContactServices
    {
        private SQLiteAsyncConnection conn;

        public async Task<bool> AddContact(Models.Contact contact)
        {
            try
            {
                if (contact != null && 
                    !string.IsNullOrEmpty(contact.FirstName) && !string.IsNullOrWhiteSpace(contact.FirstName) &&
                    !string.IsNullOrEmpty(contact.LastName) && !string.IsNullOrWhiteSpace(contact.LastName) &&
                    !string.IsNullOrEmpty(contact.Email) && !string.IsNullOrWhiteSpace(contact.Email))
                {
                    conn = new SQLiteAsyncConnection(ContactsDbConstants.ConnectionString.DatabasePath, true);
                    await conn.CreateTableAsync<Models.Contact>();
                    var result = await conn.InsertAsync(contact);
                    if (result > 0)
                    {
                        await conn.CloseAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                new ErrorMessage(ex.Message);
                return false;
            }

            return false;
        }

        public async Task<IReadOnlyList<Models.Contact>> GetContacts()
        {
            try
            {
                conn = new SQLiteAsyncConnection(ContactsDbConstants.ConnectionString.DatabasePath);
                await conn.CreateTableAsync<Models.Contact>();
                return await conn.Table<Models.Contact>().ToListAsync();
            }
            catch (Exception ex)
            {
                new ErrorMessage(ex.Message);
                await conn.CloseAsync();
                return default;
            }
        }

        public async Task<int> RemoveContact(Models.Contact contactToDelete)
        {
            int rowsAffected = 0;

            try
            {
                conn = new SQLiteAsyncConnection(ContactsDbConstants.ConnectionString.DatabasePath, true);
                await conn.CreateTableAsync<Models.Contact>();
                rowsAffected = conn.DeleteAsync<Models.Contact>(contactToDelete.Id).Result;
                await conn.CloseAsync();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                new ErrorMessage(ex.Message);
                await conn.CloseAsync();
                return rowsAffected;
            }
        }
    }
}
