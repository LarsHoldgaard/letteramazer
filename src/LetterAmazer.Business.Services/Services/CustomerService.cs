using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Interfaces;
using System;
using System.Linq;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private CustomerFactory customerFactory;
        private LetterAmazerEntities repository;
        private IPasswordEncryptor passwordEncryptor;
        private string resetPasswordUrl;
        private INotificationService notificationService;
        public CustomerService(string resetPasswordUrl, LetterAmazerEntities repository, CustomerFactory customerFactory, 
            IPasswordEncryptor passwordEncryptor, INotificationService notificationService)
        {
            this.resetPasswordUrl = resetPasswordUrl;
            this.repository = repository;
            this.passwordEncryptor = passwordEncryptor;
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

        public void CreateCustomer(Customer customer)
        {
            var dbCustomer = new DbCustomers();
            dbCustomer.Email = customer.Email.Trim().ToLower();

            if (repository.DbCustomers.Any(c => c.Email == dbCustomer.Email))
            {
                throw new BusinessException("The '" + customer.Email + "' email is existing in the system");
            }

            
            dbCustomer.DateCreated = DateTime.Now;
            dbCustomer.DateUpdated = DateTime.Now;
            dbCustomer.Credits = 0;
            
            string password = dbCustomer.Password;
            dbCustomer.Password = passwordEncryptor.Encrypt(dbCustomer.Password);

            repository.DbCustomers.Add(dbCustomer);

            repository.SaveChanges();

            customer.Password = password;
            notificationService.SendMembershipInformation(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var dbCustomer = repository.DbCustomers.FirstOrDefault(c => c.Id == customer.Id);
            dbCustomer.DateUpdated = DateTime.Now;
            dbCustomer.CustomerInfo_Address = customer.CustomerInfo.Address1;
            dbCustomer.CustomerInfo_Address2 = customer.CustomerInfo.Address1;
            dbCustomer.CustomerInfo_AttPerson = customer.CustomerInfo.Address1;
            dbCustomer.CustomerInfo_City = customer.CustomerInfo.Address1;
            dbCustomer.CustomerInfo_CompanyName = customer.CustomerInfo.Address1;
            dbCustomer.DbCountries.Id = customer.CustomerInfo.Country.Id;
            dbCustomer.CustomerInfo_FirstName = customer.CustomerInfo.FirstName;
            dbCustomer.CustomerInfo_LastName = customer.CustomerInfo.LastName;
            dbCustomer.CustomerInfo_Postal = customer.CustomerInfo.PostalCode;
            dbCustomer.CustomerInfo_VatNr = customer.CustomerInfo.VatNr;
            
            repository.SaveChanges();

        }

        public Customer GetUserByEmail(string email)
        {
            var dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.Email == email);
            if (dbcustomer == null)
            {
                throw new ItemNotFoundException("Customer");
            }

            var customer = customerFactory.Create(dbcustomer);

            return customer;
        }

        public void DeleteCustomer(int customerId)
        {

            var dbcust = repository.DbCustomers.FirstOrDefault(c => c.Id == customerId);
            repository.DbCustomers.Remove(dbcust);
            repository.SaveChanges();
        }

        public void RecoverPassword(string email)
        {
            Customer user = GetUserByEmail(email);
            user.ResetPasswordKey = Guid.NewGuid().ToString();
            string resetPasswordUrl = string.Format(this.resetPasswordUrl, System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, user.ResetPasswordKey);
            notificationService.SendResetPasswordUrl(resetPasswordUrl, user);
            repository.SaveChanges();
        }

        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Customer ValidateUser(string email, string password)
        {
            try
            {
                Customer user = GetUserByEmail(email.ToLower());
                if (user == null)
                {
                    throw new BusinessException("Email or password is not valid.");
                }

                if (!passwordEncryptor.Equal(password, user.Password))
                {
                    throw new BusinessException("Email or password is not valid.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Customer GetUserByResetPasswordKey(string resetPasswordKey)
        {
            var dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.ResetPasswordKey == resetPasswordKey);

            if (dbcustomer == null)
            {
                throw new ItemNotFoundException("Customer");
            }

            var customer = customerFactory.Create(dbcustomer);

            return customer;
        }

        public void ResetPassword(string resetPasswordKey, string newPassword)
        {
            var dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.ResetPasswordKey == resetPasswordKey);

            if (dbcustomer == null)
            {
                throw new ItemNotFoundException("Customer");
            }

            dbcustomer.ResetPasswordKey = resetPasswordKey;
            dbcustomer.ResetPasswordKey = string.Empty;

            repository.SaveChanges();

        }

        public bool IsValidCredits(int userId, decimal price)
        {
            var dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.Id == userId);
            if (dbcustomer == null) return false;

            decimal creditsLeft = dbcustomer.Credits.Value + Math.Abs(dbcustomer.CreditLimit);
            return creditsLeft >= price;
        }

        public decimal GetAvailableCredits(int userId)
        {
            var dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.Id == userId);
            if (dbcustomer == null) return 0.0m;

            return dbcustomer.Credits.Value + Math.Abs(dbcustomer.CreditLimit);
        }
    }
}
