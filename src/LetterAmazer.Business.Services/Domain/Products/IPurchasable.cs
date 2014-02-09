using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;

namespace LetterAmazer.Business.Services.Domain.Products
{
    public interface IPurchasable
    {
        ProductType ProductType();
        decimal TotalPrice();
        Customer GetCustomerDetails();
        string PurchaseText();
    }
}
