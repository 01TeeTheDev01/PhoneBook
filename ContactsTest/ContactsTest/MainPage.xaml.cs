using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;
using ContactsTest.Services;

using Xamarin.Forms;

namespace ContactsTest
{
    public partial class MainPage : ContentPage
    {
        private IContactServices contactServices;

        private ContactActions contactActions;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchContacts();
        }

        private void GetContactsBtn_Clicked(object sender, EventArgs e)
        {
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
                    var deleteContact = (Contact)ContactListView.SelectedItem;

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
                contactServices = new ContactServices();

                contactActions = new ContactActions(contactServices);

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
    }
}
