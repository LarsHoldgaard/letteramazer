using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class ApiKeysViewModel
    {
        public List<ApiKeyViewModel> ApiKeys { get; set; }

        public ApiKeysViewModel()
        {
            this.ApiKeys = new List<ApiKeyViewModel>();
        }
    }

    public class ApiKeyViewModel
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}