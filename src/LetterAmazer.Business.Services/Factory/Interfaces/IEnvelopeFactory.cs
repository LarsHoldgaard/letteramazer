using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Envelope;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IEnvelopeFactory
    {
        Envelope Create(object envelope);
        List<Envelope> Create(List<object> envelopes);
    }
}
