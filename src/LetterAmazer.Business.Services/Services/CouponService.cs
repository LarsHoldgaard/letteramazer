using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using LetterAmazer.Business.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services
{
    public class CouponService : ICouponService
    {
        private const int CouponLength = 7;
        private IRepository repository;
        private IUnitOfWork unitOfWork;
        public CouponService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool VerifyAminoUser(string username)
        {
            var urlFirst = string.Format("http://www.amino.dk/{0}", username);
            var urlSecond = string.Format("http://www.amino.dk/members/{0}/default.aspx", username);

            var respFirst = GetResponseCode(urlFirst);
            var respSecond = GetResponseCode(urlSecond);

            return respFirst || respSecond;
        }

        private bool GetResponseCode(string url)
        {

            //404: Filen ikke fundet 
            // 
            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko");
                var content = client.DownloadString(url);
                if (content.Contains("404: Filen ikke fundet") ||
                    content.Contains("Brugeren blev ikke fundet") ||
                    content.Contains("Uuups - noget gik galt!"))
                {
                    return false;
                }
            }
            return true;
        }

        public string GenerateCoupon(decimal value, int daysToExpire, string source, string sourcevalue)
        {
            return GenerateCoupon(value, daysToExpire, source, sourcevalue, 0);
        }

        public string GenerateCoupon(decimal value, int daysToExpire, string source, string sourcevalue, int earlierCoupon)
        {
            var code = PasswordGenerator.Generate(CouponLength);

            var coupon = new Coupon(){
                Code = code,
                CouponExpire = DateTime.Now.AddDays(daysToExpire),
                RefSource = source,
                RefUserValue = sourcevalue,
                CouponValue = value,
                DateGiven = DateTime.Now,
                EarlierCouponRef = earlierCoupon,
                CouponValueLeft = value,
                CouponStatus = CouponStatus.NotUsed
            };

            repository.Create<Coupon>(coupon);
            unitOfWork.Commit();

            return code;

        }

        public bool IsCouponActive(string code)
        {
            var lowerCode = code.ToLower();
            var codes = repository.Find<Coupon>(c => c.Code.ToLower() == lowerCode && c.CouponExpire >= DateTime.Now && c.CouponStatus != CouponStatus.Done && c.CouponValueLeft > 0.0m, 0, int.MaxValue, OrderBy.Desc("Id"));
            if (codes.Results.Any())
            {
                return true;
            }
            return false;
        }

        public Coupon GetCoupon(string code)
        {
            code = code.ToLower();
            if (!IsCouponActive(code))
            {
                throw new ArgumentException("Code has been used");
            }

            var currentCode = repository.FindFirst<Coupon>(c => c.Code == code);
            if (currentCode == null)
            {
                throw new ItemNotFoundException("Coupon");
            }
            return currentCode;
        }

        public decimal UseCoupon(string code, decimal price)
        {
            code = code.ToLower();
            if (!IsCouponActive(code))
            {
                throw new BusinessException("Code has been used");
            }

            var currentCode = repository.FindFirst<Coupon>(c => c.Code == code);

            if (currentCode == null)
            {
                throw new ItemNotFoundException("Coupon");
            }

            decimal differenceInValue = currentCode.CouponValueLeft- price;

            // credits left
            if (differenceInValue > 0)
            {
                currentCode.CouponValueLeft = differenceInValue;
                currentCode.CouponStatus = CouponStatus.Used;
            }
            else
            {
                currentCode.CouponStatus = CouponStatus.Done;
                currentCode.CouponValueLeft = 0;
            }

            unitOfWork.Commit();

            return differenceInValue;
        }
    }
}
