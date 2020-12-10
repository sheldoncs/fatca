using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fatca
{
    public class Variables
    {
        public const Int32 LASTNAME = 1;
        public const Int32 FIRSTNAME = 2;
        public const Int32 ACCOUNTNO = 3;
        public const Int32 TIN = 4;
        public const Int32 STREET = 5;
        public const Int32 CITY = 6;
        public const Int32 ZIP = 7;
        public const Int32 COUNTRYCODE = 8;
        //public const Int32 COUNTRY = 9;
        public const Int32 AMOUNT = 9;
        public const Int32 INTEREST = 10;
        public const Int32 DOB = 13;
        //public const Int32 TINISSUEDBY = 15;
        //public const Int32 OWNER = 16;


        public const Int32 CRS_ACCOUNTNO = 1;
        public const Int32 CRS_LASTNAME = 2;
        public const Int32 CRS_FIRSTNAME = 3;
        public const Int32 CRS_DOB = 4;
        public const Int32 CRS_TIN = 5;
        public const Int32 CRS_STREET = 6;
        public const Int32 CRS_CITY = 7;
        public const Int32 CRS_COUNTRYCODE = 8;
        public const Int32 CRS_ZIP = 9;
        public const Int32 CRS_ACCOUNT_BALANCE = 10;
        public const Int32 CRS_INTEREST_PD = 11;

        public class FatcaData
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string accountno { get; set; }
            public string tin { get; set; }
            public string street { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
            public string countrycode { get; set; }
            public string country { get; set; }
            public string amount { get; set; }
            public string interest { get; set; }
            public string dob { get; set; }
            public string resCountryCode { get; set; }
            public string tinIssuedBy { get; set; }
            public string docrefid { get; set; }
            public string currencyCode { get; set; }
            public string AcctNumberType { get; set; }
        }
        public class Country
        {
            public string code { get; set; }
            public string name { get; set; }
        }
        public class Company
        {
            public string sendingcompany { get; set; }
            public string shortaddress { get; set; }
            public string messageref { get; set; }
            public string docref { get; set; }
            public string name { get; set; }

        }
        
    }
}