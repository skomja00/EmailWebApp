using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace EmailLibrary.Model
{
    public class EmailReceipt
    {
        private static string baseUrl = "http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPI/api";
        //private static string baseUrl = "https://localhost:44349/api";

        private static string data;

        public static Boolean TagUpdate (int theAccountId,
                                    int theEmailId,
                                    string theTagName)
        {
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "/Email/TagUpdate?AccountId=" + theAccountId +
                                                                                "&EmailId=" + theEmailId +
                                                                                "&TagName=" + theTagName);
                request.Method = "PUT";
                request.ContentLength = 0;
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                data = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (data == "true")
                return true;
            else
                return false;
        }
    }
}
