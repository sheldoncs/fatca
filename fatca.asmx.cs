using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Collections;
using System.Data;

namespace fatca
{
    /// <summary>
    /// Summary description for fatca1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class fatca1 : System.Web.Services.WebService
    {
        /*
         
            using System.Xml.Linq;

            // load the file
                        var xDocument = XDocument.Load(@"C:\MyFile.xml");
            // convert the xml into string (did not get why do you want to do this)
                        string xml = xDocument.ToString();
            Now, using xDocument, you can manipulate the XML & save it back -

                       xDocument.Save(@"C:\MyFile.xml");
         */

        [WebMethod]
        public string HelloWorld()
        {
            Server.MapPath("");
            return "Hello World";
        }
         
        private string readInTemplatePath()
        {
            String path = Server.MapPath("XML/template.xml");
           
            return path;
            
        }
        private Dictionary<string, string> readInCRSISOCountryCode(String cpath)
        {
            Dictionary<string, string> Pairs = new Dictionary<string, string>();
            
            string path = Server.MapPath(cpath);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            if (doc.HasChildNodes)
            {
                XmlNodeList nodeList = doc.ChildNodes;

                foreach (XmlNode node in nodeList){
                    string str = node.LocalName;
                    if (str.Equals("schema"))
                    {
                        if (node.HasChildNodes)
                        {
                            XmlNodeList simpleTypeNodeList = node.ChildNodes;
                            foreach (XmlNode simpleTypeNode in simpleTypeNodeList)
                            {
                                String val = simpleTypeNode.LocalName;

                                if (val.Equals("simpleType"))
                                {
                                    if (simpleTypeNode.HasChildNodes)
                                    {
                                        XmlNodeList annotResrNodeList = simpleTypeNode.ChildNodes;

                                        foreach (XmlNode annotResrNode in annotResrNodeList)
                                        {
                                            if (annotResrNode.LocalName.Equals("restriction"))
                                            {
                                                if (annotResrNode.HasChildNodes)
                                                {
                                                    XmlNodeList restrictionNodeList = annotResrNode.ChildNodes;

                                                    foreach (XmlNode restrictionNode in restrictionNodeList)
                                                    {
                                                        if (restrictionNode.LocalName.Equals("enumeration"))
                                                        {
                                                            String countryCode = restrictionNode.Attributes.Item(0).Value;

                                                            XmlNode annotationNode = restrictionNode.FirstChild;

                                                            XmlNode documentationNode = annotationNode.FirstChild;

                                                            String country = documentationNode.InnerText;
                                                            //Pairs.Add(country, countryCode);
                                                            Pairs.Add(countryCode, countryCode);

                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return Pairs;
        }
        [WebMethod]
        public string readInCRSExcelFile(string fatca_xls, string SendingCompanyIN, string docrefid,
            string companyName, string periodEnding, string MessageRefId, string shortaddress, 
            string xmlfilename)
        {

            
            if (periodEnding.IndexOf("/") > 0)
            {
                periodEnding = periodEnding.Substring(6, 4) + "-" + periodEnding.Substring(0, 2) + "-" + periodEnding.Substring(3, 2);
            }
            Dictionary<string, string> countryPairs = readInCRSISOCountryCode("~/XML/CRS/countrycodes.xml");
            Dictionary<string, string> currencyPairs = readInCRSISOCountryCode("~/XML/CRS/currencycodes.xml");

            string path = Server.MapPath("~/Excel/" + fatca_xls + ".xlsx");
            //string path = Server.MapPath("~/Excel/BPW_CRS_2018.xlsx");
            List<Variables.FatcaData> list = new List<Variables.FatcaData>();
            
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

           
            Int32 rowCount = xlRange.Rows.Count;
            Int32 colCount = xlRange.Columns.Count;
            
            for (int i = 1; i <= rowCount; i++)
            {
                if (i >= 2)
                {
                    Variables.FatcaData data = new Variables.FatcaData();
                    data.accountno = FatcaLibrary.lpad(Convert.ToString(xlRange.Cells[i, Variables.CRS_ACCOUNTNO].Value2), 10); 
                    data.firstname = xlRange.Cells[i, Variables.CRS_FIRSTNAME].Value2;
                    data.firstname = data.firstname.Trim();
                    data.lastname = xlRange.Cells[i, Variables.CRS_LASTNAME].Value2;
                    data.lastname = data.lastname.Trim();

                    String obj = Convert.ToString(xlRange.Cells[i, Variables.CRS_DOB].Value2);
                    string dte = "00/00/0000";

                    try
                    {
                        
                        if (obj != "--/--/--")
                        {
                            dte = Convert.ToString(DateTime.FromOADate(xlRange.Cells[i, Variables.CRS_DOB].Value2).ToShortDateString());
                        }
                        else
                        {
                            throw new DateException("Error, Invalid Date Format for " + data.firstname+ " "+data.lastname );
                        }
                    }
                    catch (DateException ex)
                    {
                        xlWorkbook.Close();
                        Console.WriteLine(ex.Message);
                        return ex.Message;
                    } catch (Exception es)
                    {
                        //xlWorkbook.Close();
                        Console.WriteLine(es.Message);
                        dte = "1900/12/31";
                        //return es.Message + " " + data.firstname + " " + data.lastname;
                    }
                    string[] dateSplit = dte.Split('/'); 
                    data.dob = dateSplit[2] + "-" + (dateSplit[0].Length == 2 ? dateSplit[0] : FatcaLibrary.lpad(dateSplit[0], 2)) + "-" + (dateSplit[1].Length == 2 ? dateSplit[1] : FatcaLibrary.lpad(dateSplit[1], 2));

                    if (!Convert.ToString(xlRange.Cells[i, Variables.CRS_TIN].Value2).Equals("0"))
                    {
                        try
                        {
                            data.tin = FatcaLibrary.lpad(Convert.ToString(xlRange.Cells[i, Variables.CRS_TIN].Value2), 9);
                            if (data.tin.IndexOf("-") > 0)
                            {
                                if (data.tin.Length != 11)
                                {
                                    throw new TinException("Error, Invalid Tin Number for " + data.firstname+ " "+data.lastname);
                                }
                            }
                        } catch (TinException ex)
                        {
                            xlWorkbook.Close();
                            Console.WriteLine(ex.Message);
                            return ex.Message;
                        }
                    }
                    else
                    {
                        data.tin = "AAAAAAAAA";
                    }
                    String street = xlRange.Cells[i, Variables.CRS_STREET].Value2;
                    if (street != null)
                    {
                        data.street = street.Trim();
                    }
                    else
                    {
                        data.street = "NOT AVAILABLE";
                    }

                    data.city = xlRange.Cells[i, Variables.CRS_CITY].Value2;
                    if (data.city == null)
                    {
                        data.city = "";
                    }
                    else
                    {
                        data.city = data.city.ToUpper();
                    }
                    data.zip = Convert.ToString(xlRange.Cells[i, Variables.CRS_ZIP].Value2);
                    try
                    {
                        data.countrycode = xlRange.Cells[i, Variables.CRS_COUNTRYCODE].Value2;

                        if (data.countrycode == null)
                        {
                            throw new CountryCodeException("Error, Invalid Country Code at "+data.firstname+" "+data.lastname);
                        }

                    } catch (CountryCodeException cex){
                        xlWorkbook.Close();
                        return cex.Message;
                    }
                    data.city = data.city;
                    data.AcctNumberType = "OECD601";

                    try
                    {
                        data.tinIssuedBy = data.countrycode;
                        string tinIssuedBy = searchDictionary(countryPairs, data.tinIssuedBy);
                        data.currencyCode ="BBD";
                        if (tinIssuedBy.Equals(""))
                        {
                            
                            throw new CountryCodeException("Error, Invalid Country Code for " + data.firstname+ " "+data.lastname);
                        }
                    }
                    catch (CountryCodeException ex)
                    {
                        xlWorkbook.Close();
                        return ex.Message;
                    }
                    

                    double balance = Convert.ToDouble(xlRange.Cells[i, Variables.CRS_ACCOUNT_BALANCE].Value2);
                    data.amount = Convert.ToString(balance);

                    double interest = Convert.ToDouble(xlRange.Cells[i, Variables.CRS_INTEREST_PD].Value2);
                    data.interest = Convert.ToString(interest);

                    string year = Convert.ToString(Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 1);
                    //data.docrefid = SendingCompanyIN + "." + year + "0001" + FatcaLibrary.lpad(Convert.ToString(i), 10);

                    data.docrefid = data.countrycode + SendingCompanyIN + year +"_0001_" + FatcaLibrary.lpad(Convert.ToString(i), 10)+"-"+ DateTime.Now.ToString("yyyy");

                    list.Add(data);
                }
               
            }

            xlWorkbook.Close();

            FatcaLibrary.createCRSXML("", list, Server.MapPath("~/XML/"), fatca_xls, SendingCompanyIN, docrefid,
                           companyName, periodEnding, MessageRefId, shortaddress,xmlfilename);

            return "success";
        }
        [WebMethod]
        public void TestCRSFile()
        {
            //FatcaLibrary.createCRSXML("", null, Server.MapPath("~/XML/"));
        }
        private string searchDictionary( Dictionary<string, string> dict, string cntry){

            //Dictionary<string, string>.Enumerator enum = dict.
            String val = "";

            foreach (KeyValuePair<string, string> entry in dict)
            {
                String key = entry.Key.ToString();

                if (key.Equals("CANADA"))
                {
                    string CAN = key;
                }
                
                if (key.IndexOf(cntry) >= 0)
                {
                    val = entry.Value;
                }
            }

            return val;

        }
        [WebMethod]
        public String readInFatcaExcelFile(string fatca_xls,string SendingCompanyIN, string docrefid, 
            string companyName, string periodEnding, string MessageRefId,string shortaddress, 
            string xmlfilename)
        {

            if (periodEnding.IndexOf("/") > 0)
            {
                periodEnding = periodEnding.Substring(6, 4) + "-" + periodEnding.Substring(0, 2) + "-" + periodEnding.Substring(3, 2);
            }
            Dictionary<string, string> countryPairs = readInCRSISOCountryCode("~/XML/CRS/countrycodes.xml");
            Dictionary<string, string> currencyPairs = readInCRSISOCountryCode("~/XML/CRS/currencycodes.xml");
            string name = "";
            //string path = Server.MapPath("~/Excel/BPW_FATCA_2018.xlsx");
            //string path = Server.MapPath("~/Excel/" + fatca_xls);
            string path = Server.MapPath("~/Excel/" + fatca_xls + ".xlsx");
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            List<Variables.FatcaData> list = new List<Variables.FatcaData>();
            

            Int32 rowCount = xlRange.Rows.Count;
            Int32 colCount = xlRange.Columns.Count;

            for (int i = 1; i <= rowCount; i++)
            {
                if (i >= 2)
                {
                    
                        Variables.FatcaData data = new Variables.FatcaData();
                        data.lastname = xlRange.Cells[i, Variables.LASTNAME].Value2;
                        data.firstname = xlRange.Cells[i, Variables.FIRSTNAME].Value2;
                        name = data.firstname+" "+data.lastname;
                       
                        data.accountno = FatcaLibrary.lpad(Convert.ToString(xlRange.Cells[i, Variables.ACCOUNTNO].Value2), 10);

                        if (xlRange.Cells[i, Variables.TIN].Value2 != null)
                            data.tin = xlRange.Cells[i, Variables.TIN].Value2;
                        else
                            data.tin = "AAAAAAAAA";

                        data.street = xlRange.Cells[i, Variables.STREET].Value2;
                        data.street = data.street.Trim();
                        data.city = xlRange.Cells[i, Variables.CITY].Value2;
                        data.zip = Convert.ToString(xlRange.Cells[i, Variables.ZIP].Value2);
                        
                        //data.country = xlRange.Cells[i, Variables.COUNTRY].Value2;
                        data.countrycode = xlRange.Cells[i, Variables.COUNTRYCODE].Value2;
                        try
                        {
                            data.countrycode = searchDictionary(countryPairs, data.countrycode);
                            if (data.countrycode.Equals(""))
                            {
                            throw new CountryCodeException("Error, Invalid Country Code at " + name);
                            }
                        } catch(CountryCodeException ex){
                            xlWorkbook.Close();
                            Console.WriteLine(ex.Message);
                            return ex.Message;
                        }
                       
                        data.currencyCode = "BBD";
                        

                        data.amount = Convert.ToString(Convert.ToDouble(xlRange.Cells[i, Variables.AMOUNT].Value2));
                        data.interest = Convert.ToString(xlRange.Cells[i, Variables.INTEREST].Value2);
                        data.interest = Convert.ToString(Convert.ToDouble(data.interest));

                        string dte = "";
                        try
                        {

                           dte = Convert.ToString(DateTime.FromOADate(xlRange.Cells[i, Variables.DOB].Value2).ToShortDateString());

                        } catch(DateException ex)
                        {
                            xlWorkbook.Close();
                            return "Error,Invalid Date Format for " + name;
                        }
                        string[] dateSplit = dte.Split('/');
                        data.dob = dateSplit[2] + "-" + (dateSplit[0].Length == 2 ? dateSplit[0] : FatcaLibrary.lpad(dateSplit[0], 2)) + "-" + (dateSplit[1].Length == 2 ? dateSplit[1] : FatcaLibrary.lpad(dateSplit[1], 2));
                        data.resCountryCode = xlRange.Cells[i, Variables.COUNTRYCODE].Value2;
                        try
                        {
                            data.tinIssuedBy = xlRange.Cells[i, Variables.COUNTRYCODE].Value2;

                            string tinIssuedBy = searchDictionary(countryPairs, data.tinIssuedBy);
                            if (tinIssuedBy.Equals(""))
                            {
                              throw new CountryCodeException("Error, Invalid issued by for "+ name);
                            }
                        } catch (CountryCodeException ex)
                        {
                            xlWorkbook.Close();
                            return ex.Message;
                        }
                        string year = Convert.ToString(Convert.ToInt32( DateTime.Now.ToString("yyyy")) - 1);
                        data.docrefid = SendingCompanyIN + "."+year +"0001" + FatcaLibrary.lpad(Convert.ToString(i), 10)+"-"+ DateTime.Now.ToString("yyyy");
                        list.Add(data);
                        
                       
                }
            }
            xlWorkbook.Close();
            
            FatcaLibrary.createXML(readInTemplatePath(), list, Server.MapPath("~/XML/"),SendingCompanyIN,docrefid,companyName,
                                       periodEnding,MessageRefId,shortaddress, xmlfilename);
            return "success";


        }
        [WebMethod]
        public Variables.Company getCompanyDetails(string companyCode, string fileType)
        {
            DataSet ds = Library.getCompanyDetails(fileType, companyCode);

            
            int total = ds.Tables[0].Rows.Count;
            
            Variables.Company company = new Variables.Company();

            try
            {
                if (total > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        company.sendingcompany = dr["sendingcompany"].ToString();
                        company.shortaddress = dr["shortaddress"].ToString();
                        company.messageref = dr["messageref"].ToString();
                        company.docref = dr["docref"].ToString();
                        company.name = dr["name"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                String str = ex.Message;
            }
            return company;
        }
        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
        [WebMethod]
        public string GetXMLString(string xmlFileName)
        {
            XmlDocument xml = new XmlDocument();
          
            string path = Server.MapPath("~/XML/" + xmlFileName + ".xml");
            xml.Load(path);

            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xml.WriteTo(tx);

            string str = sw.ToString();
            return str;

        }
        [WebMethod]
        public string UpdateXmlDoc(string str, string filename)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            String path = Server.MapPath("~/XML/" + filename + ".xml");
            xmlDoc.Save(Console.Out);
            xmlDoc.Save(path);

            return "Successfully Updated";

        }
        [WebMethod(true)]
        public void clearSession()
        {
            if (Session != null)
            {
                Session.Clear();
            }
        }
        [WebMethod(true)]
        public Boolean confirmLogin(string username, string password)
        {
            
            
            Boolean flag = false;
            FATCA_2._0.Sessions.LoginSuccess = false;
            DataSet ds = Library.confirmLogin(username, password);
            
            int total = ds.Tables[0].Rows.Count;

            try
            {
                if (total > 0)
                {
                    
                    flag = true;
                    FATCA_2._0.Sessions.LoginSuccess = true;
                }        
            }
            catch (Exception ex)
            {
                String str = ex.Message;
            }


            return flag;
            
        }


    }
    
    
}
