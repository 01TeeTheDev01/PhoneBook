using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;
using ContactsTest.Services;
using ContactsTest.Services.PhoneBookActions;
using ContactsTest.Services.PhoneBookRepository;

using Xamarin.Forms;

using static ContactsTest.Services.PhoneBookServicesRepository;

namespace ContactsTest
{
    public partial class MainPage : ContentPage
    {
        private IPhoneBookServicesRepository contactServices;

        private PhoneBookActivities contactActions;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            FetchContacts();
        }

        private async void DeleteContactBtn_Clicked(object sender, EventArgs e)
        {
            await DeleteContactFromDB();
        }

        private async void AddContactBtn_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushModalAsync(new AddNewContactPage());
        }


        private async void FetchContacts()
        {
            await GetContactsFromDB();
        }

        private async Task DeleteContactFromDB()
        {
            try
            {
                if (ContactListView.SelectedItem != null)
                {
                    var deleteContact = (PhoneContact)ContactListView.SelectedItem;

                    ContactListView.Dispatcher.BeginInvokeOnMainThread(async () =>
                    {
                        if (await DisplayActionSheet($"Delete contact '{deleteContact.FirstName} {deleteContact.LastName}'?",
                                                     null,
                                                     null,
                                                     "Yes", "No") == "Yes")
                        {
                            if (deleteContact != null)
                            {
                                var result = await contactActions.DeleteContact(deleteContact);

                                if (result > 0)
                                {
                                    ContactListView.SelectedItem = null;
                                    ContactListView.ItemsSource = null;
                                    FetchContacts();
                                }
                                else
                                    await DisplayAlert("Information", $"{deleteContact.FirstName} {deleteContact.LastName} does not exist in database!", "I understand");
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "I understand");
            }
        }

        private async Task GetContactsFromDB()
        {
            try
            {
                contactServices = new PhoneBookServicesRepository();

                contactActions = new PhoneBookActivities(contactServices);

                //ContactListView.Dispatcher.BeginInvokeOnMainThread(() => { ContactListView.ItemsSource = null; });

                var contactsList = await contactActions.GetContactListItems();

                if (contactsList != null)
                {  
                    if (contactsList.Count > 0)
                        ContactListView.Dispatcher.BeginInvokeOnMainThread(() => { ContactListView.ItemsSource = contactsList; });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "I understand");
            }
        }

        private async void CallBtn_Clicked(object sender, EventArgs e)
        {
            var contact = (PhoneContact)ContactListView.SelectedItem;

            if (contact != null)
            {
                var result = await contactActions.CallContact(contact);

                if (result.Equals(PhoneCallStatus.Failed.ToString()))
                    await DisplayAlert("Error", ErrorMessage.Message, "I understand");
            }
        }
    }
}
