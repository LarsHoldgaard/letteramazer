using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.Envelope
{
    public class Envelope
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string ImagePath { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Thickness { get; set; }

        public int MaxPages { get; set; }

        public LetterType LetterType { get; set; }

        public decimal EnvelopeWeight { get; set; }

        public Dictionary<LetterSize, EnvelopeWindow> EnvelopeWindows { get; set; }

        public Envelope()
        {
            this.EnvelopeWindows = new Dictionary<LetterSize, EnvelopeWindow>();
        }

    }
}
