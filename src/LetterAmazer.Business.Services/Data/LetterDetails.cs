using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Utils.Enums;

namespace LetterAmazer.Business.Services.Data
{
    public class LetterDetails
    {
        public Color Color { get; set; }
        public LetterQuality LetterQuality { get; set; }
        public LetterStatus LetterStatus { get; set; }
        public PaperQuality PaperQuality { get; set; }
        public PrintQuality PrintQuality { get; set; }
        public Size Size { get; set; }
    }
}
