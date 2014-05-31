using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Envelope;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IEnvelopeFactory
    {
        Envelope Create(DbEnvelopes envelope);
        List<Envelope> Create(List<DbEnvelopes> envelopes);

        EnvelopeWindow Create(DbEnvelopeWindows envelopeWindow);
        List<EnvelopeWindow> Create(List<DbEnvelopeWindows> envelopeWindow);

    }
}
