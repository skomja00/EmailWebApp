using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmailLibrary.Model;
using Utilities;
using System.Data.SqlClient;
using System.Data;

namespace EmailCoreWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]

    public class TagsController : Controller
    {
        // GET api/Tags
        [HttpGet]
        public DataSet GetTags([FromQuery]string EmailAddress, [FromQuery]string TagType)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objSqlCmd = new SqlCommand();
            DataSet tagsDS;
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Get_Tags_SP";

            SqlParameter createdEmailAddress = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            createdEmailAddress.Direction = ParameterDirection.Input;
            createdEmailAddress.Value = EmailAddress;
            createdEmailAddress.Size = 254;
            objSqlCmd.Parameters.Add(createdEmailAddress);

            SqlParameter tagType = new SqlParameter("@TagType", SqlDbType.VarChar);
            tagType.Direction = ParameterDirection.Input;
            tagType.Value = TagType;
            tagType.Size = 6;
            objSqlCmd.Parameters.Add(tagType);

            tagsDS = objDB.GetDataSetUsingCmdObj(objSqlCmd);

            return tagsDS;
        }
        // POST api/Tags/Add
        [HttpPost("Add")]
        public Boolean Add([FromQuery]string EmailAddress, [FromBody] Tags theTag)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objSqlCmd = new SqlCommand();
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.CommandText = "Create_Tag_SP";

            SqlParameter createdEmailAddress = new SqlParameter("@CreatedEmailAddress", SqlDbType.VarChar);
            createdEmailAddress.Direction = ParameterDirection.Input;
            createdEmailAddress.Value = EmailAddress;
            createdEmailAddress.Size = 254;
            objSqlCmd.Parameters.Add(createdEmailAddress);

            SqlParameter tagName = new SqlParameter("@TagName", SqlDbType.VarChar);
            tagName.Direction = ParameterDirection.Input;
            tagName.Value = theTag.TagName;
            tagName.Size = 12;
            objSqlCmd.Parameters.Add(tagName);

            int returnValue = objDB.DoUpdateUsingCmdObj(objSqlCmd);

            if (returnValue > 0)
                return true;
            else
                return false;
        }
    }
}