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
        private IContactServices contactServices;

        private ContactActions contactActions;

        public AddNewContactPage()
        {
            InitializeComponent();

            string[] domains = { "@gmail.com", "@live.com", "@outlook.com", "@yahoo.com", "@icloud.com" };

            foreach (var domain in domains.OrderBy(d => d))
                DomainPicker.Items.Add(domain);
            
            ResetFields();
        }

        private void ResetFields()
        {
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            DomainPicker.SelectedIndex = -1;
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
                contactServices = new ContactServices();

                contactActions = new ContactActions(contactServices, new Contact());

                for (int i = 0; i < DomainPicker.Items.Count; i++)
                {
                    if (i == DomainPicker.SelectedIndex)
                    {
                        var added = await contactActions.CreateContact(FirstNameEntry.Text, LastNameEntry.Text, $"{EmailEntry.Text}{DomainPicker.Items[i]}");

                        if (added)
                        {
                            await DisplayAlert("Information", "Contact saved!", "Thank you");
                            ResetFields();
                            await Navigation.PopModalAsync(true);
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