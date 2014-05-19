using System.Collections.Generic;
using System.Reflection;
using LetterAmazer.Business.Services.Domain.Caching;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using System;
using System.Linq;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private const decimal StartCreditAmount = 3;
        private ICustomerFactory customerFactory;
        private LetterAmazerEntities repository;
        private IMailService mailService;
        private ICacheService cacheService;

        public CustomerService(LetterAmazerEntities repository, ICustomerFactory customerFactory,
            IMailService mailService,ICacheService cacheService)
        {
            this.repository = repository;
            this.customerFactory = customerFactory;
            this.mailService = mailService;
            this.cacheService = cacheService;
        }

        public Customer GetCustomerById(int customerId)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, customerId.ToString());
            if (!cacheService.ContainsKey(cacheKey))
            {
                DbCustomers dbcustomer = repository.DbCustomers.FirstOrDefault(c => c.Id == customerId);
                if (dbcustomer == null)
                {
                    throw new ItemNotFoundException("Customer");
                }

                var customer = customerFactory.Create(dbcustomer);
                cacheService.Create(cacheKey, customer);
                return customer;
            }
            return (Customer) cacheService.GetById(cacheKey);
        }

        public Customer LoginUser(string email, string password)
        {
            var lower_email = email.ToLower();
            var givenPassword = SHA1PasswordEncryptor.Encrypt(password);

            var user = repository.DbCustomers.Where(c => c.Email == lower_email && c.Password == givenPassword && c.DateActivated != null && c.DateActivated <= DateTime.Now);

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
            if (!string.IsNullOrEmpty(specification.RegistrationKey))
            {
                dbCustomers = dbCustomers.Where(c => c.RegistrationKey == specification.RegistrationKey);
            }
            if (specification.CustomerRole != null)
            {
                dbCustomers = dbCustomers.Where(c => c.OrganisationRole == (int)specification.CustomerRole);
            }

            return customerFactory.Create(dbCustomers.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
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

            mailService.ResetPassword(customer);

            Update(customer);
        }

        public void ActivateUser(Customer customer)
        {
            customer.RegisterKey = string.Empty;
            customer.DateModified = DateTime.Now;
            customer.DateActivated = DateTime.Now;
            Update(customer);
        }

        public Customer Create(Customer customer)
        {
            var providedEmail = customer.Email.ToLower().Trim();

            if (repository.DbCustomers.Any(c => c.Email == providedEmail && c.DateActivated != null && c.DateActivated <= DateTime.Now))
            {
                throw new BusinessException("The '" + providedEmail + "' email is existing in the system");
            }

            var existingcustomer = GetCustomerBySpecification(new CustomerSpecification()
            {
                Email = providedEmail
            }).FirstOrDefault();

            var givenPassword = SHA1PasswordEncryptor.Encrypt(customer.Password);

            int id = 0;
            // new user
            if (existingcustomer == null)
            {
                var dbCustomer = new DbCustomers();
                dbCustomer.Email = providedEmail;
                dbCustomer.Password = givenPassword;
                dbCustomer.CustomerInfo_FirstName = customer.CustomerInfo.FirstName;
                dbCustomer.CustomerInfo_LastName = customer.CustomerInfo.LastName;
                dbCustomer.CustomerInfo_Country = customer.CustomerInfo.Country != null ?  customer.CustomerInfo.Country.Id : 59; // TODO: Don't hardcode Denmark
                dbCustomer.DateCreated = DateTime.Now;
                dbCustomer.RegistrationKey = Guid.NewGuid().ToString();
                dbCustomer.AccountStatus = (int) (customer.AccountStatus);
                dbCustomer.Credits = StartCreditAmount;

                repository.DbCustomers.Add(dbCustomer);
                repository.SaveChanges();

                id = dbCustomer.Id;
            }
            // update old
            else
            {
                var dbCustomer = repository.DbCustomers.FirstOrDefault(c => c.Email == providedEmail);
                dbCustomer.Email = customer.Email;
                dbCustomer.Password = givenPassword;
                dbCustomer.CustomerInfo_FirstName = customer.CustomerInfo.FirstName;
                dbCustomer.CustomerInfo_LastName = customer.CustomerInfo.LastName;
                dbCustomer.RegistrationKey = Guid.NewGuid().ToString();
                dbCustomer.CustomerInfo_Country = customer.CustomerInfo.Country.Id;

                repository.SaveChanges();

                id = dbCustomer.Id;
            }


            cacheService.Delete(cacheService.GetCacheKey("GetCustomerById",id.ToString()));

            var storedCustomer = GetCustomerById(id);
            mailService.ConfirmUser(storedCustomer);
            return storedCustomer;
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

            dbCustomer.CustomerInfo_FirstName = customer.CustomerInfo.FirstName;
            dbCustomer.CustomerInfo_LastName = customer.CustomerInfo.LastName;
            dbCustomer.CustomerInfo_Zipcode = customer.CustomerInfo.Zipcode;
            dbCustomer.CustomerInfo_VatNr = customer.CustomerInfo.VatNr;
            dbCustomer.CreditLimit = customer.CreditLimit;
            dbCustomer.Credits = customer.Credit;
            dbCustomer.Email = customer.Email;
            dbCustomer.ResetPasswordKey = customer.ResetPasswordKey;
            dbCustomer.Password = customer.Password;
            dbCustomer.Phone = customer.Phone;
            dbCustomer.DateActivated = customer.DateActivated;
            dbCustomer.RegistrationKey = customer.RegisterKey;
            dbCustomer.OrganisationRole = customer.OrganisationRole.HasValue ? (int)customer.OrganisationRole.Value : 0;
            dbCustomer.AccountStatus = (int) (customer.AccountStatus);

            if (customer.Organisation != null)
            {
                dbCustomer.OrganisationId = customer.Organisation.Id;
            }
            else
            {
                dbCustomer.OrganisationId = null;
            }


            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetCustomerById", customer.Id.ToString()));

            return GetCustomerById(customer.Id);
        }

        public void Delete(Customer customer)
        {
            var dbcust = repository.DbCustomers.FirstOrDefault(c => c.Id == customer.Id);
            repository.DbCustomers.Remove(dbcust);
            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetCustomerById", customer.Id.ToString()));

        }


    }
}
