using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
