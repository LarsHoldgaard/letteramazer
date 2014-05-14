using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Files
{
    public interface IFileService
    {
        void Put(byte[] data, string path);
        byte[] Get(string path);
    }
}
