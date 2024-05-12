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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailCoreWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        DBConnect objDB = new DBConnect();

        //GET api/Account/LogIn/
        [HttpGet("LogIn")]
        [Produces("application/json")]
        public Account LogIn([FromQuery] string theLoginEmail, [FromQuery] Byte[] theLoginPass)
        {

            Account acct = new Account();
            SqlCommand objSqlCommand = new SqlCommand();
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.CommandText = "Account_Login_SP";

            SqlParameter userName = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            userName.Direction = ParameterDirection.Input;
            userName.Value = theLoginEmail;
            userName.Size = 254;
            objSqlCommand.Parameters.Add(userName);

            SqlParameter accountPassword = new SqlParameter("@AccountPassword", SqlDbType.VarBinary);
            accountPassword.Direction = ParameterDirection.Input;
            //TODO: fix theLoginPass is null problem. Remove hard code line below when fixed
            theLoginPass = [38, 115, 186, 94, 164, 122, 219, 172, 220, 69, 233, 217, 178, 239, 107, 43];
            //accountPassword.Value = Request.Query.ToDictionary()["theLoginPass"];
            accountPassword.Value = theLoginPass;
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
                acct.Avatar = Convert.ToInt32(objDB.GetField("Avatar", 0));
                acct.Active = objDB.GetField("Active", 0).ToString();
                acct.DateTimeStamp = Convert.ToDateTime(objDB.GetField("DateTimeStamp", 0));
                acct.AccountRoleType = objDB.GetField("AccountRoleType", 0).ToString();
            }

            return acct;

        }
    }
}
