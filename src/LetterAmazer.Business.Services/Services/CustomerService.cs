using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private IRepository repository;
        private IUnitOfWork unitOfWork;
        private string resetPasswordUrl;
        private INotificationService notificationService;
        public CustomerService(string resetPasswordUrl, IRepository repository, IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            this.resetPasswordUrl = resetPasswordUrl;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.notificationService = notificationService;
        }

        public Customer GetCustomerById(int customerId)
        {
            Customer customer = repository.GetById<Customer>(customerId);
            if (customer == null)
            {
                throw new ItemNotFoundException("Customer");
            }
            return customer;
        }

        public void CreateCustomer(Customer customer)
        {
            customer.Email = customer.Email.Trim().ToLower();
            if (repository.Exists<Customer>(u => u.Email == customer.Email))
            {
                throw new BusinessException("The '" + customer.Email + "' email is existing in the system");
            }
            customer.Username = customer.Email;
            customer.DateCreated = DateTime.Now;
            customer.DateUpdated = DateTime.Now;
            customer.Credits = 0;
            repository.Create(customer);
            unitOfWork.Commit();

            notificationService.SendMembershipInformation(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer current = GetCustomerById(customer.Id);
            current.DateUpdated = DateTime.Now;
            current.CustomerInfo = customer.CustomerInfo;
            repository.Update(current);
            unitOfWork.Commit();
        }

        public Customer GetUserByEmail(string email)
        {
            Customer customer = repository.FindFirst<Customer>(c => c.Email == email);
            if (customer == null)
            {
                throw new ItemNotFoundException("Customer");
            }
            return customer;
        }

        public void DeleteCustomer(int customerId)
        {
            Customer current = GetCustomerById(customerId);
            repository.Delete(current);
            unitOfWork.Commit();
        }

        public void RecoverPassword(string email)
        {
            Customer user = GetUserByEmail(email);
            user.ResetPasswordKey = Guid.NewGuid().ToString();
            string resetPasswordUrl = string.Format(this.resetPasswordUrl, System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, user.ResetPasswordKey);
            notificationService.SendResetPasswordUrl(resetPasswordUrl, user);
            repository.Update(user);
            unitOfWork.Commit();
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

                if (password != user.Password)
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
            Customer customer = repository.FindFirst<Customer>(c => c.ResetPasswordKey == resetPasswordKey);
            if (customer == null)
            {
                throw new ItemNotFoundException("Customer");
            }
            return customer;
        }

        public void ResetPassword(string resetPasswordKey, string newPassword)
        {
            Customer customer = repository.FindFirst<Customer>(c => c.ResetPasswordKey == resetPasswordKey);
            if (customer == null)
            {
                throw new ItemNotFoundException("Customer");
            }
            customer.Password = newPassword;
            customer.ResetPasswordKey = string.Empty;
            unitOfWork.Commit();
        }

        public bool IsValidCredits(int userId, decimal price)
        {
            Customer customer = repository.GetById<Customer>(userId);
            if (customer == null) return false;

            decimal creditsLeft = customer.Credits.Value + Math.Abs(customer.CreditLimit);
            return creditsLeft >= price;
        }

        public decimal GetAvailableCredits(int userId)
        {
            Customer customer = repository.GetById<Customer>(userId);
            return customer.Credits.Value + Math.Abs(customer.CreditLimit);
        }
    }
}
