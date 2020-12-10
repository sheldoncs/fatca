using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace fatca
{
    [Serializable]
    public class CountryCodeException : Exception
    {
        public CountryCodeException()
        {
        }

        public CountryCodeException(string message): base(message)
        {
        }
    }
}