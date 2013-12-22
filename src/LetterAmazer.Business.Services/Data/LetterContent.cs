using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Data
{
    public class LetterContent
    {
        public string Path { get; set; }

        public byte[] Content { get; set; }

        public string WrittenContent { get; set; }
    }
}
