using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Exceptions
{
    [global::System.Serializable]
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() { }
        public ItemNotFoundException(string message) : base(message) { }
        public ItemNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ItemNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public ItemNotFoundException(string type, string key) 
            : base(string.Format("{0} '{1}' not found.", type, key)) 
        { 
        }

        public ItemNotFoundException(string type, int objId)
            : base(string.Format("{0} #{1} not found.", type, objId))
        {
        }
    }
}
