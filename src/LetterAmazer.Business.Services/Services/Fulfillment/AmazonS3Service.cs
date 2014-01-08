using System.IO;
using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Data;

namespace LetterAmazer.Business.Services.Services.Fulfillment
{
    public class AmazonS3Service
    {
        private AmazonS3 client;
        private string accessKeyID;
        private string secretAccessKeyID;
        private AmazonS3Config config;

        public AmazonS3Service()
        { 
        }
        
        public AmazonS3Service(string accessKey, string secret, string serviceUrl)
            : this()
        {
            this.accessKeyID = accessKey;
            this.secretAccessKeyID = secret;
            this.config = new AmazonS3Config();
            this.config.ServiceURL = serviceUrl;
        }

        public void SendSQSMessage(string msg, string queue)
        {
            using (var client =new AmazonSQSClient(accessKeyID, secretAccessKeyID))
            {
                try
                {
                    var m = new SendMessageRequest()
                    {
                        MessageBody = msg,
                        QueueUrl = queue
                    };
                    var resp = client.SendMessage(m);

                    var own = HelperMethods.HashFile(new MemoryStream(HelperMethods.GetBytes(msg)));
                    var amazonResp = resp.SendMessageResult.MD5OfMessageBody;
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        //log exception - ("Please check the provided AWS Credentials.");
                    }
                    else
                    {
                        //log exception - ("An Error, number {0}, occurred when creating a bucket with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);    
                    }
                }
            }
        }

        public void CreateBucket(string bucketName)
        {
            using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID, config))
            {
                try
                {
                    PutBucketRequest request = new PutBucketRequest();
                    request.WithBucketName(bucketName).WithBucketRegion(S3Region.EU);

                    client.PutBucket(request);
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        //log exception - ("Please check the provided AWS Credentials.");
                    }
                    else
                    {
                        //log exception - ("An Error, number {0}, occurred when creating a bucket with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);    
                    }
                }
            }
        }

        public void UploadFile(string bucketName, Stream uploadFileStream, string remoteFileName, string md5)
        {
            using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID, config))
            {
                try
                {
                    StringBuilder stringResp = new StringBuilder();

                    PutObjectRequest request = new PutObjectRequest();
                   // request.MD5Digest = md5;
                    request.BucketName = bucketName;
                    request.InputStream = uploadFileStream;
                    request.Key = remoteFileName;
                    request.MD5Digest = md5;
                    
                    using (S3Response response = client.PutObject(request))
                    {
                        WebHeaderCollection headers = response.Headers;
                        foreach (string key in headers.Keys)
                        {
                            stringResp.AppendLine(string.Format("Key: {0}, value: {1}", key,headers.Get(key).ToString()));
                            //log headers ("Response Header: {0}, Value: {1}", key, headers.Get(key));
                        }
                    }
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        //log exception - ("Please check the provided AWS Credentials.");
                    }
                    else
                    {
                        //log exception -("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
                    }
                }
            }
        }
    }
}
