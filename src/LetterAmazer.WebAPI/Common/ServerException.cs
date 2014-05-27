using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.WebAPI.Common
{
    /// <summary>
    /// Server exception Business object
    /// </summary>
    public class ServerException
    {
        /// <summary>
        /// What server threw the exception
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Timestamp of when exception occurred
        /// </summary>
        public DateTime ExceptionTime { get; set; }

        /// <summary>
        /// Exception Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Exception Stack Trace
        /// </summary>
        public string StackTrace { get; set; }

        public static ServerException GenerateTestObject()
        {
            return new ServerException
            {
                ExceptionTime = DateTime.Now,
                Message = "test",
                StackTrace = "test",
                Server = "test",
            };
        }
    }
}