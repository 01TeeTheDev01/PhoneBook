using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;

namespace ContactsTest.Services
{
    internal interface IContactServices
    {
        Task<IReadOnlyList<Contact>> GetContacts();
        Task<bool> AddContact(Contact contact);
        Task<int> RemoveContact(Contact contact);
    }
}
