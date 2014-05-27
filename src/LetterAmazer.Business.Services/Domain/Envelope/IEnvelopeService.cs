using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Envelope
{
    public interface IEnvelopeService
    {
        Envelope Create(Envelope envelope);
        Envelope GetById(int id);
        Envelope Update(Envelope envelope);

    }
}
