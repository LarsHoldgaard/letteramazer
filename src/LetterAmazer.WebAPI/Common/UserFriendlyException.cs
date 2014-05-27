using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.WebAPI.Common
{
    /// <summary>
    /// Throw a UserFriendly exception to expose a user-friendly message to
    /// the calling client.
    /// </summary>
    public class UserFriendlyException : Exception
    {
        public ErrorCode ErrorCode { get; set; }
        public ErrorThrownBy ThrownBy { get; set; }

        public UserFriendlyException(string userFriendlyMessage)
            : base(userFriendlyMessage)
        {
        }

        public UserFriendlyException(string userFriendlyMessage, Exception innerExcepetion)
            : base(userFriendlyMessage, innerExcepetion)
        {
        }

        public UserFriendlyException(string userFriendlyMessage, ErrorCode errorCode, Exception innerExcepetion)
            : this(userFriendlyMessage, innerExcepetion)
        {
            ErrorCode = errorCode;
        }

        public UserFriendlyException(string userFriendlyMessage, ErrorThrownBy application, ErrorCode errorCode, Exception innerExcepetion)
            : this(userFriendlyMessage, errorCode, innerExcepetion)
        {
            ThrownBy = application;
        }

        public UserFriendlyException(string userFriendlyMessage, ErrorCode errorCode)
            : this(userFriendlyMessage)
        {
            ErrorCode = errorCode;
        }

        public UserFriendlyException(string userFriendlyMessage, ErrorThrownBy application, ErrorCode errorCode)
            : this(userFriendlyMessage, errorCode)
        {
            ThrownBy = application;
        }
    }
}