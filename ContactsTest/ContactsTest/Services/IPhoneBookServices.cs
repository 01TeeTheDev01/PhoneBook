using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;

namespace ContactsTest.Services
{
    internal interface IPhoneBookServices
    {
        Task<IReadOnlyList<PhoneContact>> GetContacts();
        Task<bool> AddContact(PhoneContact contact);
        Task<int> RemoveContact(PhoneContact contact);
        Task<string> PlaceCall(PhoneContact contact);
    }
}
