using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Envelope
{
    public class EnvelopeWindow
    {
        public decimal WindowXOffset { get; set; }
        public decimal WindowYOffset { get; set; }
        public decimal WindowLength { get; set; }
        public decimal WindowHeight { get; set; }
    }
}
