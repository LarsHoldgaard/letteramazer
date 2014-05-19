using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.AWSSupport;
using Amazon.S3;
using Amazon.S3.Model;
using iTextSharp.text.pdf.crypto;
using LetterAmazer.Business.Services.Domain.Caching;
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
        private ICacheService cacheService;

        public FileService(ICacheService cacheService)
        {
            this.accessId = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.AccessKeyId"];
            this.secretAccessId = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.SecretAccessKey"];
            this.bucketname = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.Bucketname"];
            this.cacheService = cacheService;
        }



        public byte[] GetFileById(string path)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, path);
            if (!cacheService.ContainsKey(cacheKey))
            {
                using (var client = new AmazonS3Client(accessId, secretAccessId))
                {
                    var obj = client.GetObject(new GetObjectRequest()
                    {
                        BucketName = bucketname,
                        Key = path
                    });
                    var fileBytes = Helpers.GetBytes(obj.ResponseStream);

                    cacheService.Create(cacheKey, fileBytes);
                    return fileBytes;
                }
            }
            return (byte[])cacheService.GetById(cacheKey);

        }

        public string Create(byte[] data, string path)
        {
            path = getUploadDateString(path);
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

            cacheService.Delete(cacheService.GetCacheKey("GetFileById",path));
            return path;
        }


        private string getUploadDateString(string path)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month,path);
        }
    }
}
