﻿using System.Collections.Generic;
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
        private IPasswordEncryptor passwordEncryptor;
        private INotificationService notificationService;
        public CustomerService(LetterAmazerEntities repository, ICustomerFactory customerFactory, 
            IPasswordEncryptor passwordEncryptor, INotificationService notificationService)
        {
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

        public List<Customer> GetCustomerBySpecification(CustomerSpecification specification)
        {
            IQueryable<DbCustomers> dbCustomers = repository.DbCustomers;

            if (specification.Id > 0)
            {
                dbCustomers = dbCustomers.Where(c => c.Id == specification.Id);
            }
            if (!string.IsNullOrEmpty(specification.Email))
            {
                dbCustomers = dbCustomers.Where(c => c.Email == specification.Email);
            }
            if (!string.IsNullOrEmpty(specification.ResetPasswordKey))
            {
                dbCustomers = dbCustomers.Where(c => c.ResetPasswordKey == specification.ResetPasswordKey);
            }

            return customerFactory.Create(dbCustomers.ToList());
        }

        public void Create(Customer customer)
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

        public void Update(Customer customer)
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

      

        public void Delete(Customer customer)
        {
            var dbcust = repository.DbCustomers.FirstOrDefault(c => c.Id == customer.Id);
            repository.DbCustomers.Remove(dbcust);
            repository.SaveChanges();
        }

      
    }
}
