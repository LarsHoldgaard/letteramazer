using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Files
{
    public interface IFileService
    {
        string Create(byte[] data, string path);
        string Create(byte[] data, string path,FileUploadMode mode);
        byte[] GetFileById(string path);
        byte[] GetFileById(string path, FileUploadMode mode);
    }
}
