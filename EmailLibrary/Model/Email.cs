using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace EmailLibrary.Model
{
    public class Email
    {
        private string baseUrl = "http://cis-iis2.temple.edu/spring2021/cis3342_tun49199/WebAPI/api";
        //private string baseUrl = "https://localhost:44349/api";

        private int emailId;
        private string sendAccount;
        private string recvAccountList;
        private string emailSubject;
        private string emailBody;
        private DateTime dateTimeStamp;
        
        DBConnect objDB = new DBConnect();
        SqlCommand objSqlCmd = new SqlCommand();
        DataSet emailDS;
        public Email ()
        {

        }
        /// <summary>
        /// GetEmail creates DataSet (i.e. DataSet emailDS) 
        /// with email for a given email address in a given folder.
        /// Note: The DataSet is also stored as a class variable for 
        /// use in other class methods
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="theTagName"></param>
        /// <returns>DataSet</returns>
        public DataSet GetEmail(string theCreatedEmailAddress, string theTagName)
        {
            WebRequest request = WebRequest.Create(baseUrl + "/Email/GetEmail?sendEmailAddress=" + theCreatedEmailAddress + "&tagName=" + theTagName);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String json = reader.ReadToEnd();
            reader.Close();
            response.Close();
            emailDS = JsonConvert.DeserializeObject<DataSet>(json);
            return emailDS;
        }
        /// <summary>
        /// GetSentEmail creates DataSet (i.e. DataSet emailDS) 
        /// with email from a given email address. Note
        /// GetSentEmail works slightly different the by 
        /// selecting based on the Email.AccountId
        /// rather than the current tag assigned to the email. 
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="theTagName"></param>
        /// <returns>DataSet</returns>
        public DataSet GetSentEmail(string theCreatedEmailAddress)
        {
            WebRequest request = WebRequest.Create(baseUrl + "/Email/GetSentEmail?sendEmailAddress=" + theCreatedEmailAddress);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String json = reader.ReadToEnd();
            reader.Close();
            response.Close();
            emailDS = JsonConvert.DeserializeObject<DataSet>(json);
            return emailDS;
        }
        /// <summary>
        /// GetEmailWithTag selects all emails with the given tag. 
        /// </summary>
        /// <param name="theTagName"></param>
        /// <returns></returns>
        public DataSet GetEmailWithTag(string theTagName)
        {
            WebRequest request = WebRequest.Create(baseUrl + "/Email/GetEmailWithTag?tagName=" + theTagName);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String json = reader.ReadToEnd();
            reader.Close();
            response.Close();
            emailDS = JsonConvert.DeserializeObject<DataSet>(json);
            return emailDS;
        }
        /// <summary>
        /// Builds a DataTable from the DataRow[] returned from the DataTable.Select 
        /// method using theSearchString filter criteria.
        /// </summary>
        /// <param name="theSearchString">The criteria to use to filter the rows.</param>
        /// <returns>DataTable</returns>
        public DataTable SearchEmail(string theSearchString)
        {
            string col;
            DataRow row;

            // For more info on DataTable Class see following link:
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-5.0
            DataTable objSearchDT = emailDS.Tables[0];
            DataRow[] objRows = objSearchDT.Select(theSearchString);
            DataTable objResultsDT = new DataTable("SearchResults");

            // If search found something build a search results DataTable
            if (objRows.Count() > 0 )
            {
                // add the Columns 
                for (int c = 0; c < objRows[0].Table.Columns.Count; c++)
                {
                    objResultsDT.Columns.Add(objRows[0].Table.Columns[c].ToString());
                }
                // add the data to the Rows collection
                for (int r = 0; r < objRows.Count(); r++)
                {
                    row = objResultsDT.NewRow();
                    for (int c = 0; c < objRows[r].ItemArray.Count(); c++)
                    {
                        col = objResultsDT.Columns[c].ColumnName;
                        row[col] = objRows[r].ItemArray[c].ToString();
                    }
                    objResultsDT.Rows.Add(row);
                }
            }
            return objResultsDT;
        }
        public Boolean SendEmail ()
        {
            string data;
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "/Email/SendEmail");
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
        /// <summary>
        /// Return the emailDS DataSet 
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetEmailDS()
        {
            return emailDS;
        }
        public int EmailId 
        {
            get { return emailId; }
            set { emailId = value; }
        }
        public string SendAccount
        {
            get { return sendAccount; }
            set { sendAccount = value; }
        }
        public string RecvAccountList
        {
            get { return recvAccountList; }
            set { recvAccountList = value; }
        }
        public string EmailSubject
        {
            get { return emailSubject; }
            set { emailSubject = value; }
        }
        public string EmailBody
        {
            get { return emailBody; }
            set { emailBody = value; }
        }
        public DateTime DateTimeStamp
        {
            get { return dateTimeStamp; }
            set { dateTimeStamp = value; }
        }
    }
}
