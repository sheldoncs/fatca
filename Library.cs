using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace fatca
{
    public class Library
    {
        public static string getConnectionString()
        {
            string connStr = "";

            connStr = "SERVER=" + "50.62.209.9" + ";" + "DATABASE=" +
                        "opitin_realestate" + ";" + "port=3306;" + "UID=" + "sheldoncs" + ";" + "PASSWORD=" + "Kentish@46" + ";";

            return connStr;
        }
        public static DataSet getCompanyDetails(string filetype, string companycode)
        {
            DataSet ds = new DataSet();


            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = getConnectionString();

                String cmdString = String.Format(@"select distinct a.sendingcompany,shortaddress,messageref,docref, b.companyname as name " +
                                                   "from companydetails a, company b " +
                                                     "where a.compcode = b.compcode " +
                                                     " and a.filetype = b.filetype " +
                                                     "and a.filetype = '{0}' and a.compcode = '{1}'", filetype, companycode);



                using (MySqlCommand cmd = new MySqlCommand(cmdString, conn))
                {

                    using (MySqlDataAdapter data = new MySqlDataAdapter())
                    {
                        data.SelectCommand = cmd;

                        conn.Open();
                        data.Fill(ds);

                        try
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                return ds;
                            }
                        }
                        catch (Exception Ex)
                        {
                            String msg = Ex.Message;
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return ds;
        }
        public static DataSet confirmLogin(String username, String password)
        {

            DataSet ds = new DataSet();


            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = getConnectionString();

                String cmdString = String.Format(@"select username, password " +
                    "from login " +
                    "where username = '{0}' and password = '{1}'", username, password);



                using (MySqlCommand cmd = new MySqlCommand(cmdString, conn))
                {

                    using (MySqlDataAdapter data = new MySqlDataAdapter())
                    {
                        data.SelectCommand = cmd;

                        conn.Open();
                        data.Fill(ds);

                        try
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                return ds;
                            }
                        }
                        catch (Exception Ex)
                        {
                            String msg = Ex.Message;
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return ds;
        }

    }

}