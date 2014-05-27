using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Envelope;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class EnvelopeFactory:IEnvelopeFactory
    {
        public Envelope Create(DbEnvelopes envelope)
        {
            return new Envelope()
            {
                EnvelopeWeight = envelope.EnvelopeWeightInGm.HasValue ? envelope.EnvelopeWeightInGm.Value : 0.0m,
                Height = envelope.HeightInCm.HasValue ? envelope.HeightInCm.Value : 0.0m,
                Thickness = envelope.ThicknessInCm.HasValue ? envelope.ThicknessInCm.Value : 0.0m,
                MaxPages = envelope.MaxPages.HasValue ? envelope.MaxPages.Value : 0,
                Length = envelope.EnvelopeWeightInGm.HasValue ? envelope.EnvelopeWeightInGm.Value : 0.0m,
                LetterType = (LetterType)envelope.EnvelopeType,
                Id = envelope.Id,
                ImagePath = envelope.ImagePath,
                Label = envelope.Label
            };
        }

        public List<Envelope> Create(List<DbEnvelopes> envelopes)
        {
            return envelopes.Select(Create).ToList();
        }
    }
}
