using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Common
{
    public class S3Access
    {
        public string User { get; set; }
        public string KeyId { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }
        public string PostQueue { get; set; }
    }
}
