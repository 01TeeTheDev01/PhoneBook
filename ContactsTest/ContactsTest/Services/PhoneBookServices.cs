using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ContactsTest.Models;

using SQLite;

using Xamarin.Essentials;

namespace ContactsTest.Services
{
    internal partial class PhoneBookServices : IPhoneBookServices
    {
        private SQLiteAsyncConnection conn;

        public async Task<bool> AddContact(PhoneContact contact)
        {
            try
            {
                if (contact != null && 
                    !string.IsNullOrEmpty(contact.FirstName) && !string.IsNullOrWhiteSpace(contact.FirstName) &&
                    !string.IsNullOrEmpty(contact.LastName) && !string.IsNullOrWhiteSpace(contact.LastName) &&
                    !string.IsNullOrEmpty(contact.Email) && !string.IsNullOrWhiteSpace(contact.Email) &&
                    !string.IsNullOrEmpty(contact.PhoneNumber) && !string.IsNullOrWhiteSpace(contact.PhoneNumber))
                {
                    var emailRegex = new Regex(PhoneContactConstants.EmailRegex).IsMatch(contact.Email);

                    var phoneRegex = new Regex(PhoneContactConstants.PhoneNumberRegex).IsMatch(contact.PhoneNumber);

                    if (emailRegex && phoneRegex)
                    {
                        conn = new SQLiteAsyncConnection(PhoneContactConstants.ConnectionString.DatabasePath, true);
                        await conn.CreateTableAsync<Models.PhoneContact>();
                        var result = await conn.InsertAsync(contact);
                        if (result > 0)
                        {
                            await conn.CloseAsync();
                            return true;
                        }
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

        public async Task<IReadOnlyList<PhoneContact>> GetContacts()
        {
            try
            {
                conn = new SQLiteAsyncConnection(PhoneContactConstants.ConnectionString.DatabasePath);
                await conn.CreateTableAsync<Models.PhoneContact>();
                await conn.CloseAsync();
                return await conn.Table<Models.PhoneContact>().ToListAsync();
            }
            catch (Exception ex)
            {
                new ErrorMessage(ex.Message);
                await conn.CloseAsync();
                return default;
            }
        }

        public async Task<string> PlaceCall(PhoneContact contact)
        {
            try
            {
                PhoneDialer.Open(contact.PhoneNumber);

                return await Task.FromResult(PhoneCallStatus.Passed.ToString());
            }
            catch (Exception e)
            {
                new ErrorMessage(e.Message);
                return PhoneCallStatus.Failed.ToString();
            }
        }

        public async Task<int> RemoveContact(PhoneContact contactToDelete)
        {
            try
            {
                conn = new SQLiteAsyncConnection(PhoneContactConstants.ConnectionString.DatabasePath, true);
                await conn.CreateTableAsync<PhoneContact>();
                await conn.CloseAsync();
                return await conn.DeleteAsync<PhoneContact>(contactToDelete.Id);
            }
            catch (Exception ex)
            {
                new ErrorMessage(ex.Message);
                await conn.CloseAsync();
                return 0;
            }
        }
    }
}
