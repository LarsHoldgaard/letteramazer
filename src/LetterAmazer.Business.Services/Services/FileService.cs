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
        private string temp_bucketname;

        private ICacheService cacheService;

        public FileService(ICacheService cacheService)
        {
            this.accessId = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.AccessKeyId"];
            this.secretAccessId = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.SecretAccessKey"];
            this.bucketname = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.Bucketname"];
            this.temp_bucketname = ConfigurationManager.AppSettings["LetterAmazer.Storage.S3.Temporarily_Bucketname"];

            this.cacheService = cacheService;
        }


        public byte[] GetFileById(string path)
        {
            return GetFileById(path, FileUploadMode.Permanently);
        }

        public byte[] GetFileById(string path,FileUploadMode mode)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, path + "_" + mode.ToString());
            if (!cacheService.ContainsKey(cacheKey))
            {
                using (var client = new AmazonS3Client(accessId, secretAccessId))
                {
                    string bucketName = bucketname;
                    if (mode == FileUploadMode.Temporarily)
                    {
                        bucketName = temp_bucketname;
                    }
                    
                    var obj = client.GetObject(new GetObjectRequest()
                    {
                        BucketName = bucketName,
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
            return Create(data, path, FileUploadMode.Permanently);
        }

        public string Create(byte[] data, string path, FileUploadMode mode)
        {
            using (var client = new AmazonS3Client(accessId, secretAccessId))
            {
                string bucketName = bucketname;
                if (mode == FileUploadMode.Temporarily)
                {
                    bucketName = temp_bucketname;
                }

                client.PutObject(new PutObjectRequest()
                {
                    BucketName = bucketName,
                    ContentType = "application/pdf",
                    Key = path,
                    InputStream = new MemoryStream(data)
                });
            }

            cacheService.Delete(cacheService.GetCacheKey("GetFileById",path));
            return path;
        }



    }
}
