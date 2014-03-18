using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class EditContactsViewModel
    {
        public int OrganisationId { get; set; }

        public List<ContactViewModel> Contacts { get; set; }

        public ContactViewModel NewContact { get; set; }

        public EditContactsViewModel()
        {
            this.NewContact = new ContactViewModel();
           this.Contacts = new List<ContactViewModel>();
        }
    }

    public class ContactViewModel
    {
        public string OrganisationName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string VatNumber { get; set; }

        public int AddressListId { get; set; }

        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public ContactViewModel()
        {
            this.Countries = new List<SelectListItem>();
        }
    }
}