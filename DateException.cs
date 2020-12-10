using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace fatca
{
    [Serializable]
    public class DateException : Exception
    {
        public DateException()
        {
        }

        public DateException(string message): base(message)
        {
        }
    }
}