using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class ApiKeysViewModel
    {
        public List<ApiKeyViewModel> ApiKeys { get; set; }

        public ApiKeyViewModel NewApiKey { get; set; }

        public ApiKeysViewModel()
        {
            this.NewApiKey = new ApiKeyViewModel();
            this.ApiKeys = new List<ApiKeyViewModel>();
        }
    }

    public class ApiKeyViewModel
    {
        public string Label { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}