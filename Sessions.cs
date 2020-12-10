using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FATCA_2._0
{
    public class Sessions
    {
        public static Boolean LoginSuccess
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["LoginSuccess"] != null)
                {
                    return (Boolean)System.Web.HttpContext.Current.Session["LoginSuccess"];
                }
                return false;
            }
            set
            {
                System.Web.HttpContext.Current.Session["LoginSuccess"] = value;
            }
        }
    }
}