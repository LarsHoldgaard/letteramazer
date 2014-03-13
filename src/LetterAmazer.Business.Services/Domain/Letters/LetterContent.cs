using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Utils.Helpers;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public class LetterContent
    {
        private ICountryService countryService;

        public LetterContent(ICountryService countryService)
        {
            this.countryService = countryService;
        }
       
        public string Path { get; set; }
        public byte[] Content { get; set; }
        public string WrittenContent { get; set; }

        public int PageCount
        {
            get
            {
                var pages = PdfHelper.GetPagesCount(PathHelper.GetAbsoluteFile(Path));
                return pages;
            }
        }

        public string TextInPdf
        {
            get { return string.Empty; }
        }

        public Country GetFirstCountryInPdf()
        {
            var allCountryNames = countryService.GetCountryNamesBySpecification(new CountryNameSpecification()
            {
                Take = 99999
            }).Select(c=>c.Name);

            var textInPdf = TextInPdf;
            var words = textInPdf.Split(' ');

            foreach (var word in words)
            {
                if (allCountryNames.Contains(word))
                {
                    // win
                }
            }

            return null;
        }
    }
}
