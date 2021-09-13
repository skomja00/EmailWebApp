using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Newtonsoft.Json;
using System.IO;

namespace EmailLibrary.Model
{
    public class Tags
    {
        private static string baseUrl = "http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPI/api";
        //private static string baseUrl = "https://localhost:44349/api";

        private int tagId;
        private string tagName;
        private string tagType;
        private DateTime dateTimeStamp;

        public Tags ()
        {
        
        }
        public static DataSet GetTagsDataSet (string theCreatedEmailAddress, string theTagType)
        {
            WebRequest request = WebRequest.Create(baseUrl + "/Tags" + "?EmailAddress=" + theCreatedEmailAddress + "&TagType=" + theTagType);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            DataSet tagsDS = JsonConvert.DeserializeObject<DataSet>(data);

            return tagsDS;
        }
        public Boolean Add (string theCreatedEmailAddress)
        {
            string data;

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            
            WebRequest request = WebRequest.Create(baseUrl + "/Tags/Add" + "?EmailAddress=" + theCreatedEmailAddress);
            request.Method = "POST";
            request.ContentLength = json.Length;
            request.ContentType = "application/json";

            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(json);
            writer.Flush();
            writer.Close();

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            if (data == "true")
                return true;
            else
                return false;
        }
        public int TagId
        {
            get { return tagId; }
            set { tagId = value; }
        }
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }
        public string TagType
        {
            get { return tagType; }
            set { tagType = value; }
        }
        public DateTime DateTimeStamp
        {
            get { return dateTimeStamp; }
            set { dateTimeStamp = value; }
        }

    }
}
