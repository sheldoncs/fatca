using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.HttpContext.Current.Server;

namespace FATCA_2._0.excel
{
    /// <summary>
    /// Summary description for ExcelHandler
    /// </summary>
    public class ExcelHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string docType = context.Request.QueryString["docType"];

           

            
           
            //write your handler implementation here.
            context.Response.ContentType = "application/vnd.ms-excel";



            if (context.Request.Files.Count <= 0)
            {
                context.Response.Write("No file uploaded");
            }
            else
            {
                for (int i = 0; i <= context.Request.Files.Count; ++i)
                {
                    if (i >= 1)
                    {

                        HttpPostedFile file = context.Request.Files[i - 1];

                        string extension = System.IO.Path.GetExtension(file.FileName.ToString());
                        if (docType.Equals("bpwcrs"))
                        {
                            if (extension.Equals(".xlsx"))
                            {

                                try
                                {

                                    string s = context.Server.MapPath("/excel/bpw_crs" + extension);
                                    file.SaveAs(s);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("bpw_crs");

                                }
                                catch (Exception ex)
                                {
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("the error is = " + ex.Message.ToString());
                                }
                                break;

                            }
                            else
                            {
                                context.Response.ContentType = "text/plain";
                                context.Response.Write("error, Document Type Should be xlsx");
                                break;

                            }
                        }
                        else if (docType.Equals("capitacrs"))
                        {

                            if (extension.Equals(".xlsx"))
                            {
                                try
                                {
                                    
                                    string s = context.Server.MapPath("/excel/capita_crs" + extension);
                                    file.SaveAs(s);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("capita_crs");
                                    break;

                                }
                                catch (Exception ex)
                                {
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("error, " + ex.Message.ToString());

                                    break;
                                }
                            }
                            else
                            {

                                context.Response.ContentType = "text/plain";
                                context.Response.Write("error, file of incorrect type:Required xlsx");

                            }

                        } else if (docType.Equals("capitafatca"))
                        {

                            if (extension.Equals(".xlsx"))
                            {
                                try
                                {
                                    string s = context.Server.MapPath("/excel/capita_fatca" + extension);
                                    file.SaveAs(s);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("capita_fatca");
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("error, " + ex.Message.ToString());

                                    break;
                                }
                            }
                            else
                            {

                                context.Response.ContentType = "text/plain";
                                context.Response.Write("error, file of incorrect type:Required xlsx");

                            }


                        }
                        else if (docType.Equals("bpwfatca"))
                        {

                            if (extension.Equals(".xlsx"))
                            {
                                try
                                {
                                   
                                    string s = context.Server.MapPath("/excel/bpw_fatca" + extension);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("bpw_fatca");
                                    file.SaveAs(s);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("error, " + ex.Message.ToString());

                                    break;
                                }
                            }
                            else
                            {

                                context.Response.ContentType = "text/plain";
                                context.Response.Write("error, file of incorrect type:Required xlsx");

                            }

                        }

                        file.InputStream.Flush();
                        file.InputStream.Dispose();
                        file.InputStream.Close();
                    }
                    
                }
            }
         
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}