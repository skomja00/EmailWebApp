using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using System.Data;
using EmailLibrary.Model;

namespace EmailCoreWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]

    public class EmailController : Controller
    {
        DBConnect objDB = new DBConnect();

        //GET api/Email/GetEmailWithTag/
        [HttpGet("GetEmailWithTag")]
        [Produces("application/json")]
        public DataSet GetEmailWithTag([FromQuery]string tagName)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Get_Email_With_Tag_SP";

            SqlParameter tagNameParm = new SqlParameter("@TagName", SqlDbType.VarChar);
            tagNameParm.Direction = ParameterDirection.Input;
            tagNameParm.Value = tagName;
            tagNameParm.Size = 254;
            objSqlCmd.Parameters.Add(tagNameParm);

            DataSet emailDS = new DataSet();
            emailDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);
            emailDS.Tables[0].TableName = "GetEmailsWithTag";

            return emailDS;
        }
        //GET api/Email/GetEmail/
        [HttpGet("GetEmail")]
        [Produces("application/json")]
        public DataSet GetEmail([FromQuery]string sendEmailAddress, [FromQuery]string tagName)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Get_Email_SP";

            SqlParameter emailAddressParm = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            emailAddressParm.Direction = ParameterDirection.Input;
            emailAddressParm.Value = sendEmailAddress;
            emailAddressParm.Size = 254;
            objSqlCmd.Parameters.Add(emailAddressParm);

            SqlParameter tagNameParm = new SqlParameter("@TagName", SqlDbType.VarChar);
            tagNameParm.Direction = ParameterDirection.Input;
            tagNameParm.Value = tagName;
            tagNameParm.Size = 254;
            objSqlCmd.Parameters.Add(tagNameParm);

            DataSet emailDS = new DataSet();
            emailDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);
            emailDS.Tables[0].TableName = "GetEmail";

            return emailDS;
        }
        //GET api/Email/GetSentEmail/
        [HttpGet("GetSentEmail")]
        [Produces("application/json")]
        public DataSet GetSentEmail([FromQuery]string sendEmailAddress)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Get_Sent_Email_SP";

            SqlParameter emailAddressParm = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            emailAddressParm.Direction = ParameterDirection.Input;
            emailAddressParm.Value = sendEmailAddress;
            emailAddressParm.Size = 254;
            objSqlCmd.Parameters.Add(emailAddressParm);

            DataSet emailDS = new DataSet();
            emailDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);
            emailDS.Tables[0].TableName = "GetEmail";

            return emailDS;
        }
        //PUT api/Email/TagUpdate/
        [HttpPut("TagUpdate")]
        public Boolean TagUpdate([FromQuery]int AccountId, [FromQuery]int EmailId, [FromQuery]string TagName)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "EmailRecipt_Tag_Update_SP";

            SqlParameter accountIdParm = new SqlParameter("@AccountId", SqlDbType.Int);
            accountIdParm.Direction = ParameterDirection.Input;
            accountIdParm.SqlDbType = SqlDbType.Int;
            accountIdParm.Value = AccountId;
            accountIdParm.Size = 4;
            objSqlCmd.Parameters.Add(accountIdParm);

            SqlParameter emailIdParm = new SqlParameter("@EmailId", SqlDbType.Int);
            emailIdParm.Direction = ParameterDirection.Input;
            emailIdParm.SqlDbType = SqlDbType.Int;
            emailIdParm.Value = EmailId;
            emailIdParm.Size = 4;
            objSqlCmd.Parameters.Add(emailIdParm);

            SqlParameter tagNameParm = new SqlParameter("@TagName", SqlDbType.VarChar);
            tagNameParm.Direction = ParameterDirection.Input;
            tagNameParm.SqlDbType = SqlDbType.VarChar;
            tagNameParm.Value = TagName;
            tagNameParm.Size = 6;
            objSqlCmd.Parameters.Add(tagNameParm);

            int returnVal = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnVal > 0)
                return true;
            else
                return false;
        }
        //POST api/Email/SendEmail/
        [HttpPost("SendEmail")]
        public Boolean SendEmail([FromBody] Email theEmail)
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Email_Send_SP";

            SqlParameter sendEmailAddress = new SqlParameter("@SendEmailAddress", SqlDbType.VarChar);
            sendEmailAddress.Direction = ParameterDirection.Input;
            sendEmailAddress.Value = theEmail.SendAccount;
            sendEmailAddress.Size = 254;
            objSqlCmd.Parameters.Add(sendEmailAddress);

            SqlParameter recvEmailList = new SqlParameter("@RecvEmailList", SqlDbType.VarChar);
            recvEmailList.Direction = ParameterDirection.Input;
            recvEmailList.Value = theEmail.RecvAccountList;
            recvEmailList.Size = 4094;
            objSqlCmd.Parameters.Add(recvEmailList);

            SqlParameter emailSubject = new SqlParameter("@EmailSubject", SqlDbType.VarChar);
            emailSubject.Direction = ParameterDirection.Input;
            emailSubject.Value = theEmail.EmailSubject;
            emailSubject.Size = 254;
            objSqlCmd.Parameters.Add(emailSubject);

            SqlParameter emailBody = new SqlParameter("@EmailBody", SqlDbType.VarChar);
            emailBody.Direction = ParameterDirection.Input;
            emailBody.Value = theEmail.EmailBody;
            objSqlCmd.Parameters.Add(emailBody);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
    }
}
