using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using System.Data;
using EmailLibrary.Model;
using System.Text;
using System.Xml.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailCoreWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        DBConnect objDB = new DBConnect();

        //POST api/Account/LogIn/
        [HttpPost("LogIn")]
        [Produces("application/json")]
        public Account LogIn([FromBody] Account theAccount)
        {
            Account acct = new Account();
            SqlCommand objSqlCommand = new SqlCommand();
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.CommandText = "Account_Login_SP";

            SqlParameter userName = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            userName.Direction = ParameterDirection.Input;
            userName.Value = theAccount.CreatedEmailAddress;
            userName.Size = 254;
            objSqlCommand.Parameters.Add(userName);

            SqlParameter accountPassword = new SqlParameter("@AccountPassword", SqlDbType.VarBinary);
            accountPassword.Direction = ParameterDirection.Input;
            accountPassword.Value = theAccount.AccountPassword;
            objSqlCommand.Parameters.Add(accountPassword);

            // Execute stored procedure to login. 
            DataSet objDS = objDB.GetDataSetUsingCmdObj(objSqlCommand);

            int rowCount = objDS.Tables[0].Rows.Count;

            if (rowCount > 0)
            {
                acct.AccountId = Convert.ToInt32(objDB.GetField("AccountId", 0));
                acct.UserName = objDB.GetField("UserName", 0).ToString();
                acct.UserAddress = objDB.GetField("UserAddress", 0).ToString();
                acct.PhoneNumber = objDB.GetField("PhoneNumber", 0).ToString();
                acct.CreatedEmailAddress = objDB.GetField("CreatedEmailAddress", 0).ToString();
                acct.ContactEmailAddress = objDB.GetField("ContactEmailAddress", 0).ToString();
                acct.AccountPassword = Encoding.UTF8.GetBytes(objDB.GetField("AccountPassword",0).ToString());
                acct.Avatar = Convert.ToInt32(objDB.GetField("Avatar", 0));
                acct.Active = objDB.GetField("Active", 0).ToString();
                acct.DateTimeStamp = Convert.ToDateTime(objDB.GetField("DateTimeStamp", 0));
                acct.AccountRoleType = objDB.GetField("AccountRoleType", 0).ToString();
            }

            return acct;

        }
        //POST api/Account/SecurityQuestions
        /// <summary>
        /// Check the count of responses to the Security Question match the database.
        /// Return the number of matching responses.
        /// </summary>
        /// <returns>
        /// Integer Number of matching responses
        /// </returns>
        [HttpPost("SecurityQuestions")]
        [Produces("application/json")]
        public int SecurityQuestions([FromBody] Account theAccount)
        {
            SqlCommand objSqlCmd = new SqlCommand();

            objSqlCmd.CommandText = "Account_Security_Questions_SP";
            objSqlCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", theAccount.CreatedEmailAddress);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.SqlDbType = SqlDbType.VarChar;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter cityParm = new SqlParameter("@ResponseCity", theAccount.SecurityQuestionCity);
            cityParm.Direction = ParameterDirection.Input;
            cityParm.SqlDbType = SqlDbType.VarChar;
            cityParm.Size = 254;
            objSqlCmd.Parameters.Add(cityParm);

            SqlParameter phoneParm = new SqlParameter("@ResponsePhone", theAccount.SecurityQuestionPhone);
            phoneParm.Direction = ParameterDirection.Input;
            phoneParm.SqlDbType = SqlDbType.VarChar;
            phoneParm.Size = 254;
            objSqlCmd.Parameters.Add(phoneParm);

            SqlParameter schoolParm = new SqlParameter("@ResponseSchool", theAccount.SecurityQuestionSchool);
            schoolParm.Direction = ParameterDirection.Input;
            schoolParm.SqlDbType = SqlDbType.VarChar;
            schoolParm.Size = 254;
            objSqlCmd.Parameters.Add(schoolParm);

            DataSet objDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

            int numOfCorrectResponses = Convert.ToInt32(objDS.Tables[0].Rows[0].ItemArray[0]);

            return numOfCorrectResponses;
        }
        //PUT api/Account/UpdatePassword
        /// <summary>
        /// Execute the Account_Update_Password_SP stored procedure to update the AccountPassword
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="thePassword"></param>
        /// <returns>
        /// Integer number of rows affected by the update, or -1 for an exception
        /// </returns>
        [HttpPut("UpdatePassword")]
        [Produces("application/json")]
        public int UpdatePassword([FromBody] Account theAccount)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objSqlCmd = new SqlCommand();

            objSqlCmd.CommandText = "Account_Update_Password_SP";
            objSqlCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter emailParm = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            emailParm.Direction = ParameterDirection.Input;
            emailParm.Value = theAccount.CreatedEmailAddress;
            emailParm.Size = 254;
            objSqlCmd.Parameters.Add(emailParm);

            SqlParameter passwordParm = new SqlParameter("@AccountPassword", SqlDbType.VarBinary);
            passwordParm.Direction = ParameterDirection.Input;
            passwordParm.Value = theAccount.AccountPassword;
            //passwordParm.Size = 32;
            objSqlCmd.Parameters.Add(passwordParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            return returnValue;
        }
        //POST api/Account/CreateAccount
        /// Create an account.
        /// Return the result of the data base return code.
        /// </summary>
        /// <returns>
        /// Integer data base return code
        /// </returns>
        [HttpPost("CreateAccount")]
        [Produces("application/json")]
        public int CreateAccount([FromBody] Account theAccount)
        {
            SqlCommand objSqlCmd = new SqlCommand();

            objSqlCmd.CommandType = CommandType.StoredProcedure;

            objSqlCmd.CommandText = "Account_Insert_SP";

            // Parameters are as follows:
            SqlParameter userName = new SqlParameter("@UserName", SqlDbType.VarChar);
            userName.Direction = ParameterDirection.Input;
            userName.Value = theAccount.UserName;
            userName.Size = 50;
            objSqlCmd.Parameters.Add(userName);

            SqlParameter userAddress = new SqlParameter("@UserAddress", SqlDbType.VarChar);
            userAddress.Direction = ParameterDirection.Input;
            userAddress.Value = theAccount.UserAddress;
            userAddress.Size = 200;
            objSqlCmd.Parameters.Add(userAddress);

            SqlParameter phoneNumber = new SqlParameter("@PhoneNumber", SqlDbType.VarChar);
            phoneNumber.Direction = ParameterDirection.Input;
            phoneNumber.Value = theAccount.PhoneNumber;
            phoneNumber.Size = 50;
            objSqlCmd.Parameters.Add(phoneNumber);

            SqlParameter createdEmailAddress = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            createdEmailAddress.Direction = ParameterDirection.Input;
            createdEmailAddress.Value = theAccount.CreatedEmailAddress;
            createdEmailAddress.Size = 200;
            objSqlCmd.Parameters.Add(createdEmailAddress);

            SqlParameter contactEmailAddress = new SqlParameter("@ContactEmailAddress", SqlDbType.VarChar);
            contactEmailAddress.Direction = ParameterDirection.Input;
            contactEmailAddress.Value = theAccount.ContactEmailAddress;
            contactEmailAddress.Size = 200;
            objSqlCmd.Parameters.Add(contactEmailAddress);

            SqlParameter avatar = new SqlParameter("@Avatar", SqlDbType.Int);
            avatar.Direction = ParameterDirection.Input;
            avatar.Value = theAccount.Avatar;
            avatar.Size = 4;
            objSqlCmd.Parameters.Add(avatar);

            SqlParameter accountPassword = new SqlParameter("@AccountPassword", SqlDbType.VarBinary);
            accountPassword.Direction = ParameterDirection.Input;
            accountPassword.Value = theAccount.AccountPassword;
            objSqlCmd.Parameters.Add(accountPassword);

            SqlParameter active = new SqlParameter("@Active", SqlDbType.VarChar);
            active.Direction = ParameterDirection.Input;
            active.Value = "yes";
            active.Size = 5;
            objSqlCmd.Parameters.Add(active);

            SqlParameter DateTimeStamp = new SqlParameter("@DateTimeStamp", SqlDbType.DateTime);
            DateTimeStamp.Direction = ParameterDirection.Input;
            DateTimeStamp.Value = DateTime.Now;
            DateTimeStamp.Size = 8; // 8 bytes
            objSqlCmd.Parameters.Add(DateTimeStamp);

            SqlParameter responseCity = new SqlParameter("@ResponseCity", SqlDbType.VarChar);
            responseCity.Direction = ParameterDirection.Input;
            responseCity.Value = theAccount.SecurityQuestionCity;
            responseCity.Size = 254;
            objSqlCmd.Parameters.Add(responseCity);

            SqlParameter responsePhone = new SqlParameter("@ResponsePhone", SqlDbType.VarChar);
            responsePhone.Direction = ParameterDirection.Input;
            responsePhone.Value = theAccount.SecurityQuestionPhone;
            responsePhone.Size = 254;
            objSqlCmd.Parameters.Add(responsePhone);

            SqlParameter responseSchool = new SqlParameter("@ResponseSchool", SqlDbType.VarChar);
            responseSchool.Direction = ParameterDirection.Input;
            responseSchool.Value = theAccount.SecurityQuestionSchool;
            responseSchool.Size = 254;
            objSqlCmd.Parameters.Add(responseSchool);

            SqlParameter accountRoleType = new SqlParameter("@AccountRoleType", SqlDbType.VarChar);
            accountRoleType.Direction = ParameterDirection.Input;
            accountRoleType.Value = theAccount.AccountRoleType;
            accountRoleType.Size = 14;
            objSqlCmd.Parameters.Add(accountRoleType);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            return returnValue;
        }
        //PUT api/Account/BanUnban
        [HttpPut("BanUnban")]
        [Produces("application/json")]
        public int BanUnban([FromBody] Account theAccount)
        {            
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Account_Active_Update_SP";

            SqlParameter accountIdParm = new SqlParameter("@AccountId", SqlDbType.Int);
            accountIdParm.Direction = ParameterDirection.Input;
            accountIdParm.Value = theAccount.AccountId;
            accountIdParm.Size = 4;
            objSqlCmd.Parameters.Add(accountIdParm);

            SqlParameter activeParm = new SqlParameter("@Active", SqlDbType.VarChar);
            activeParm.Direction = ParameterDirection.Input;
            activeParm.Value = theAccount.Active;
            activeParm.Size = 5;
            objSqlCmd.Parameters.Add(activeParm);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            return returnValue;
        }
        //GET api/Account/FlaggedEmail
        [HttpGet("FlaggedEmail")]
        [Produces("application/json")]
        public DataSet FlaggedEmail()
        {
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Get_Accounts_With_Flagged_Email_SP";

            DataSet accountDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

            return accountDS;
        }
    }
}
