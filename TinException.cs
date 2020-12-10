using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace fatca
{
    [Serializable]
    public class TinException : Exception
    {
        public TinException()
        {
        }

        public TinException(string message): base(message)
        {
        }
    }
}