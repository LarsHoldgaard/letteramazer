using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Caching;
using LetterAmazer.Business.Services.Domain.Envelope;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class EnvelopeService:IEnvelopeService
    {
          private LetterAmazerEntities repository;
        private IEnvelopeFactory envelopeFactory;
        private ICacheService cacheService;
        public EnvelopeService(LetterAmazerEntities repository, IEnvelopeFactory envelopeFactory,ICacheService cacheService)
        {
            this.envelopeFactory = envelopeFactory;
            this.repository = repository;
            this.cacheService = cacheService;
        }


        public Envelope Create(Envelope envelope)
        {
            throw new NotImplementedException();
        }

        public Envelope GetEnvelopeById(int id)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, id.ToString());

            if (!cacheService.ContainsKey(cacheKey))
            {
                var dbEnvelope = repository.DbEnvelopes.FirstOrDefault(c => c.Id == id);

                if (dbEnvelope == null)
                {
                    throw new ArgumentException("No envelope by this id");
                }

                var envelope = envelopeFactory.Create(dbEnvelope);

                var dbEnvelopeWindows = repository.DbEnvelopeWindows.Where(c => c.EnvelopeId == envelope.Id).ToList();
                var envelopeWindows = envelopeFactory.Create(dbEnvelopeWindows);

                foreach (var envel in envelopeWindows)
                {
                    envelope.EnvelopeWindows.Add(envel.LetterSize,envel);
                }

                cacheService.Create(cacheKey, envelope);
                return envelope;
            }
            return (Envelope)cacheService.GetById(cacheKey);

        }

        public Envelope Update(Envelope envelope)
        {
            throw new NotImplementedException();
        }
    }
}
