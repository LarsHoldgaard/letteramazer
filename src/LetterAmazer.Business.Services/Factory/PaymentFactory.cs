using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class PaymentFactory : IPaymentFactory
    {
        public PaymentMethods Create(DbPaymentMethods dbPaymentMethods)
        {
            return new PaymentMethods()
            {
                DateCreated = dbPaymentMethods.DateCreated,
                DateDeleted= dbPaymentMethods.DateDeleted,
                Id = dbPaymentMethods.Id,
                IsVisible = dbPaymentMethods.IsVisible,
                MaximumAmount = dbPaymentMethods.MaximumAmount,
                MinimumAmount = dbPaymentMethods.MinimumAmount,
                Price = dbPaymentMethods.Price,
                Name = dbPaymentMethods.Name,
                SortOrder = dbPaymentMethods.SortOrder,
                RequiresLogin = dbPaymentMethods.RequiresLogin,
                Label = dbPaymentMethods.Label,
                LogoPath = dbPaymentMethods.LogoPath
            };
        }

        public List<PaymentMethods> Create(List<DbPaymentMethods> dbPaymentMethodses)
        {
            return dbPaymentMethodses.Select(Create).ToList();
        }
    }
}
