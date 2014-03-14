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

        private string textinpdf;
        public string TextInPdf
        {
            get
            {
                if (string.IsNullOrEmpty(textinpdf))
                {
                    textinpdf = PdfHelper.GetContentsOfPdf(PathHelper.GetAbsoluteFile(Path));
                }
                return textinpdf;
            }
        }

        public Country GetFirstCountryInPdf(List<CountryName> countryNamesList)
        {
            var countryNames = countryNamesList.Select(c => c.Name);
            var textInPdf = TextInPdf;
            var words = textInPdf.Split(' ');

            foreach (var word in words)
            {
                if (countryNames.Contains(word))
                {
                    int i = 0;
                }
            }

            return null;
        }
    }
}
