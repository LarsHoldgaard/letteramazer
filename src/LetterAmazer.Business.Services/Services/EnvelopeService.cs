using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Envelope;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class EnvelopeService:IEnvelopeService
    {
          private LetterAmazerEntities repository;
        private IEnvelopeFactory envelopeFactory;

        public EnvelopeService(LetterAmazerEntities repository, IEnvelopeFactory envelopeFactory)
        {
            this.envelopeFactory = envelopeFactory;
            this.repository = repository;
        }


        public Envelope Create(Envelope envelope)
        {
            throw new NotImplementedException();
        }

        public Envelope GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Envelope Update(Envelope envelope)
        {
            throw new NotImplementedException();
        }
    }
}
