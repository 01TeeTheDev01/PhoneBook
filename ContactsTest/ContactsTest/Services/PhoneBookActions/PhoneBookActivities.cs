using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ContactsTest.Models;
using ContactsTest.Services.PhoneBookRepository;

namespace ContactsTest.Services.PhoneBookActions
{
    internal class PhoneBookActivities
    {
        private readonly IPhoneBookServicesRepository _contactServices;

        private readonly PhoneContact contactObject;

        public PhoneBookActivities(IPhoneBookServicesRepository contactServices)
        {
            _contactServices = contactServices ?? throw new ArgumentNullException(nameof(contactServices));
        }

        public PhoneBookActivities(IPhoneBookServicesRepository contactServices, PhoneContact contactObject = null)
        {
            _contactServices = contactServices ?? throw new ArgumentNullException(nameof(contactServices));
            this.contactObject = contactObject ?? throw new ArgumentNullException(nameof(contactObject));
        }

        public async Task<IReadOnlyList<PhoneContact>> GetContactListItems()
        {
            return await _contactServices.GetContacts();
        }

        public async Task<bool> CreateContact(string firstName, string lastName, string email, string phone)
        {
            contactObject.FirstName = firstName;
            contactObject.LastName = lastName;
            contactObject.Email = email;
            contactObject.PhoneNumber = phone;

            return await _contactServices.AddContact(contactObject);
        }

        public async Task<int> DeleteContact(PhoneContact deleteObject)
        {
            return await _contactServices.RemoveContact(deleteObject);
        }

        public async Task<string> CallContact(PhoneContact contact)
        {
            return await _contactServices.PlaceCall(contact);
        }
    }
}
