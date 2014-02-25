using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Notifications;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using System;
using System.Linq;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerFactory customerFactory;
        private LetterAmazerEntities repository;
        private INotificationService notificationService;
        public CustomerService(LetterAmazerEntities repository, ICustomerFactory customerFactory, 
            INotificationService notificationService)
        {
            this.repository = repository;
            this.notificationService = notificationService;
            this.customerFactory = customerFactory;
        }

        public Customer GetCustomerById(int customerId)
        {
            DbCustomers dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.Id == customerId);
            if (dbcustomer == null)
            {
                throw new ItemNotFoundException("Customer");
            }

            var customer = customerFactory.Create(dbcustomer);

            return customer;
        }

        public Customer LoginUser(string email, string password)
        {
            var lower_email = email.ToLower();

            var user = repository.DbCustomers.Where(c => c.Email == lower_email && c.Password == password);

            if (!user.Any())
            {
                return null;
            }

            var first_user = user.FirstOrDefault();

            return customerFactory.Create(first_user);
        }

        public List<Customer> GetCustomerBySpecification(CustomerSpecification specification)
        {
            IQueryable<DbCustomers> dbCustomers = repository.DbCustomers;

            if (specification.Id > 0)
            {
                dbCustomers = dbCustomers.Where(c => c.Id == specification.Id);
            }
            if (!string.IsNullOrEmpty(specification.Email))
            {
                var selectedEmail = specification.Email.ToLower();
                dbCustomers = dbCustomers.Where(c => c.Email == selectedEmail);
            }
            if (!string.IsNullOrEmpty(specification.ResetPasswordKey))
            {
                dbCustomers = dbCustomers.Where(c => c.ResetPasswordKey == specification.ResetPasswordKey);
            }

            return customerFactory.Create(dbCustomers.OrderBy(c=>c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public void RecoverPassword(string email)
        {
            var customer = GetCustomerBySpecification(new CustomerSpecification()
            {
                Email = email
            }).FirstOrDefault();

            if (customer == null)
            {
                throw new BusinessException("Customer doesn't exist");
            }

            customer.ResetPasswordKey = Guid.NewGuid().ToString();
            //string resetPasswordUrl = string.Format(this.resetPasswordUrl, System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, user.ResetPasswordKey);
            
            notificationService.SendResetPasswordUrl(string.Empty, customer);

            Update(customer);
        }

        public Customer Create(Customer customer)
        {
            var dbCustomer = new DbCustomers();
            dbCustomer.Email = customer.Email.Trim().ToLower();

            if (repository.DbCustomers.Any(c => c.Email == dbCustomer.Email))
            {
                throw new BusinessException("The '" + customer.Email + "' email is existing in the system");
            }

            dbCustomer.DateCreated = DateTime.Now;
            dbCustomer.DateUpdated = DateTime.Now;
            dbCustomer.Credits = customer.Credit;
            dbCustomer.CreditLimit = customer.CreditLimit;

            if (customer.CustomerInfo != null)
            {
                dbCustomer.CustomerInfo_Address = customer.CustomerInfo.Address1;
                dbCustomer.CustomerInfo_Address2 = customer.CustomerInfo.Address2;
                dbCustomer.CustomerInfo_AttPerson = customer.CustomerInfo.AttPerson;
                dbCustomer.CustomerInfo_City = customer.CustomerInfo.City;
                dbCustomer.CustomerInfo_CompanyName = customer.CustomerInfo.Organisation;
                dbCustomer.CustomerInfo_FirstName = customer.CustomerInfo.FirstName;
                dbCustomer.CustomerInfo_LastName = customer.CustomerInfo.LastName;
                dbCustomer.CustomerInfo_Postal = customer.CustomerInfo.PostalCode;
                dbCustomer.CustomerInfo_VatNr = customer.CustomerInfo.VatNr;


                if (customer.CustomerInfo.Country != null)
                {
                    dbCustomer.DbCountries.Id = customer.CustomerInfo.Country.Id;
                }
            }
            
            


            string password = customer.Password;
            dbCustomer.Password = SHA1PasswordEncryptor.Encrypt(password);

            repository.DbCustomers.Add(dbCustomer);

            repository.SaveChanges();

            customer.Password = password;
            //notificationService.SendMembershipInformation(customer);

            return GetCustomerById(dbCustomer.Id);
        }

        public Customer Update(Customer customer)
        {
            var dbCustomer = repository.DbCustomers.FirstOrDefault(c => c.Id == customer.Id);

            if (dbCustomer == null)
            {
                throw new BusinessException("The customer doesn't exist");
            }

            dbCustomer.DateUpdated = DateTime.Now;
            dbCustomer.CustomerInfo_Address = customer.CustomerInfo.Address1;
            dbCustomer.CustomerInfo_Address2 = customer.CustomerInfo.Address2;
            dbCustomer.CustomerInfo_AttPerson = customer.CustomerInfo.AttPerson;
            dbCustomer.CustomerInfo_City = customer.CustomerInfo.City;
            dbCustomer.CustomerInfo_CompanyName = customer.CustomerInfo.Organisation;
            //dbCustomer.DbCountries.Id = customer.CustomerInfo.Country != null ? customer.CustomerInfo.Country.Id : 0;
            dbCustomer.CustomerInfo_FirstName = customer.CustomerInfo.FirstName;
            dbCustomer.CustomerInfo_LastName = customer.CustomerInfo.LastName;
            dbCustomer.CustomerInfo_Postal = customer.CustomerInfo.PostalCode;
            dbCustomer.CustomerInfo_VatNr = customer.CustomerInfo.VatNr;
            dbCustomer.CreditLimit = customer.CreditLimit;
            dbCustomer.Credits = customer.Credit;
            dbCustomer.Email = customer.Email;
            dbCustomer.ResetPasswordKey = customer.ResetPasswordKey;
            dbCustomer.Password = customer.Password;
            dbCustomer.Phone = customer.Phone;
            


            repository.SaveChanges();

            return GetCustomerById(customer.Id);
        }

      

        public void Delete(Customer customer)
        {
            var dbcust = repository.DbCustomers.FirstOrDefault(c => c.Id == customer.Id);
            repository.DbCustomers.Remove(dbcust);
            repository.SaveChanges();
        }

      
    }
}
