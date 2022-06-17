using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ContactsTest.Models;
using ContactsTest.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactsTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewContactPage : ContentPage
    {
        private IPhoneBookServices contactServices;

        private PhoneBookActions contactActions;

        public AddNewContactPage()
        {
            InitializeComponent();

            string[] domains = { "@gmail.com", "@live.com", "@outlook.com", "@yahoo.com", "@icloud.com" };

            string[] countryCode = {"+27"};

            foreach (var domain in domains.OrderBy(d => d))
                DomainPicker.Items.Add(domain);

            foreach (var country in countryCode)
                CountryPicker.Items.Add(country);
            
            ResetFields();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DomainPicker.SelectedIndex = 0;
        }

        private void ResetFields()
        {
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PhoneEntry.Text = string.Empty;
            DomainPicker.SelectedIndex = -1;
            CountryPicker.SelectedIndex = -1;
        }

        private async void BackToMainBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "I understand");
            }
        }

        private async void CreateContactBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                contactServices = new PhoneBookServices();

                contactActions = new PhoneBookActions(contactServices, new PhoneContact());

                for (int domainIndex = 0; domainIndex < DomainPicker.Items.Count; domainIndex++)
                {
                    if (domainIndex == DomainPicker.SelectedIndex)
                    {
                        for (int countryIndex = 0; countryIndex < CountryPicker.Items.Count; countryIndex++)
                        {
                            if (countryIndex == CountryPicker.SelectedIndex)
                            {
                                var added = await contactActions.CreateContact(firstName: FirstNameEntry.Text,
                                                                                   lastName: LastNameEntry.Text,
                                                                                   email: $"{EmailEntry.Text}{DomainPicker.Items[countryIndex]}",
                                                                                   phone: $"{CountryPicker.Items[countryIndex]}{PhoneEntry.Text}");

                                if (added)
                                {
                                    await DisplayAlert("Information", "Contact saved!", "Thank you");
                                    await Navigation.PopModalAsync(true);
                                    ResetFields();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "I understand");
            }
        }
    }
}
