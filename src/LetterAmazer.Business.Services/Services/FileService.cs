using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.AWSSupport;
using Amazon.S3;
using Amazon.S3.Model;
using iTextSharp.text.pdf.crypto;
using LetterAmazer.Business.Services.Domain.Files;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Utils;
using LetterAmazer.Business.Utils.Helpers;

namespace LetterAmazer.Business.Services.Services
{
    public class FileService:IFileService
    {
        private string accessId;
        private string secretAccessId;
        private string bucketname;

        public FileService()
        {
            this.accessId = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.AccessKeyId"];
            this.secretAccessId = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.SecretAccessKey"];
            this.bucketname = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.Bucketname"];

        }



        public byte[] Get(string path)
        {
            using (var client = new AmazonS3Client(accessId, secretAccessId))
            {
                var obj = client.GetObject(new GetObjectRequest()
                {
                    BucketName = bucketname,
                    Key = path
                });
                return Helpers.GetBytes(obj.ResponseStream);
            }
        }

        public void Put(byte[] data, string path)
        {
            using (var client = new AmazonS3Client(accessId, secretAccessId))
            {
                client.PutObject(new PutObjectRequest()
                {
                    BucketName = bucketname,
                    ContentType = "application/pdf",
                    Key = path,
                    InputStream = new MemoryStream(data)
                });
            }
        }
    }
}
