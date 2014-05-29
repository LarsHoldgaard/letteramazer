using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Services;
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
                return PdfHelper.GetPagesCount(Content);
            }
        }

        private string textinpdf;
        public string TextInPdf
        {
            get
            {
                throw new NotImplementedException();
                if (string.IsNullOrEmpty(textinpdf))
                {
                    //textinpdf = PdfHelper.GetContentsOfPdf(PathHelper.GetAbsoluteFile(Path));
                }
                return textinpdf;
            }
        }

        public int GetFirstCountryIdInPdf(List<CountryName> countryNamesList)
        {
            var countryNames = countryNamesList;
            //var countryNames = countryNamesList.Select(c => c.Name);
            var textInPdf = TextInPdf;
            var words = textInPdf.Split(' ');

            foreach (var word in words.Select(HelperMethods.RemoveSpecialCharacters))
            {
                var p_word = word.ToLower();
                
                if (countryNames.Any(c=>c.Name == p_word))
                {
                    return countryNames.FirstOrDefault(c => c.Name == p_word).CountryId;
                }
            }
            return 0;
        }
    }
}
