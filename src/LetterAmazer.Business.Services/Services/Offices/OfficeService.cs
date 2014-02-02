using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services.Offices
{
    public class OfficeService:IOfficeService
    {
        private IRepository repository;
        private IUnitOfWork unitOfWork;

        public OfficeService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public Office GetOfficeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id has to be above 0");
            }

            var office = repository.GetById<Office>(id);

            if (office == null)
            {
                throw new ItemNotFoundException("office");
            }

            return office;
        }

        public List<Office> GetOfficeBySpecification(OfficeSpecification specification)
        {
            //var data = repository.Where<IQueryable<Office>>(c=>c.FirstOrDefault().);
            return new List<Office>();
            //IQueryable<Office> officeList = repository.Offices;

            //if (specification.CountryId > 0)
            //{
            //    officeList = repository.Offices.Where(c => c.CountryId == specification.CountryId);
            //}
            //if (specification.LetterSize != null)
            //{
            //    officeList =
            //        officeList.Where(
            //            c => c.OfficeProducts.Any(d => d.OfficeProductDetail.Size == (int)specification.LetterSize));
            //}

            //return officeList.ToList();
        }
    }
}
