using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;

namespace ContactsTest.Services
{
    internal class ContactActions
    {
        private readonly IContactServices _contactServices;

        private readonly Contact contactObject;

        public ContactActions(IContactServices contactServices)
        {
            _contactServices = contactServices ?? throw new ArgumentNullException(nameof(contactServices));
        }

        public ContactActions(IContactServices contactServices, Contact contact)
        {
            _contactServices = contactServices ?? throw new ArgumentNullException(nameof(contactServices));

            contactObject = contact ?? throw new ArgumentNullException(nameof(contact));
        }

        public async Task<IReadOnlyList<Contact>> GetContactListItems()
        {
            return await _contactServices.GetContacts();
        }

        public async Task<bool> CreateContact(string firstName, string lastName, string email)
        {
            contactObject.FirstName = firstName;
            contactObject.LastName = lastName;
            contactObject.Email = email;

            return await _contactServices.AddContact(contactObject);
        }

        public async Task<int> DeleteContact(Contact deleteObject)
        {
            return await _contactServices.RemoveContact(deleteObject);
        }
    }
}
