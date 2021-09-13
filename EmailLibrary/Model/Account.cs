using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;

namespace EmailLibrary.Model
{
    [Serializable]
    public class Account
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objSqlCmd = new SqlCommand();

        //Create a web service proxy object
        //using the AccountWeb.asmx page
        AccountService.AccountWeb pxy = new AccountService.AccountWeb();

        //Create an object of the of the AccountService.Account class
        //using the AccountService and WSDL
        AccountService.Account objAccount = new AccountService.Account();

        private int accountId;
        private string userName;
        private string userAddress;
        private string phoneNumber;
        private string createdEmailAddress;
        private string contactEmailAddress;
        private int avatar;
        private Byte[] accountPassword;
        private string active; // "yes" or "no" 
        private string securityQuestionCity;
        private string securityQuestionPhone;
        private string securityQuestionSchool;
        private DateTime dateTimeStamp;
        private string accountRoleType;
        private static Byte[] key =    { 250, 101, 018, 076, 045, 135, 207, 118, 004, 171, 003, 168, 202, 241, 037, 199 };
        private static Byte[] vector = { 146, 064, 191, 111, 023, 003, 113, 119, 231, 121, 252, 112, 079, 032, 114, 156 };
        
        public Account LogIn(string theCreatedEmailAddress, Byte[] theAccountPassword)
        {
            objAccount = pxy.LogIn(theCreatedEmailAddress, theAccountPassword);

            if (objAccount.AccountId > 0)
            {
                InitializeThis();
            }

            return this;
        }
        public DataSet GetAccountsWithFlaggedEmail()
        {
            DataSet flaggedEmails = pxy.GetAccountsWithFlaggedEmail();

            return flaggedEmails;
        }
        public int BanUnban()
        {
            objAccount.AccountId = this.AccountId;
            objAccount.Active = this.Active;

            int result = pxy.BanUnBan(objAccount);

            return result;
        }
        public int CreateAccount ()
        {
            objAccount.AccountId = this.AccountId;
            objAccount.UserName = this.UserName;
            objAccount.UserAddress = this.UserAddress;
            objAccount.PhoneNumber = this.PhoneNumber;
            objAccount.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccount.ContactEmailAddress = this.ContactEmailAddress;
            objAccount.Avatar = this.Avatar;
            objAccount.AccountPassword = this.AccountPassword;
            objAccount.Active = this.Active;
            objAccount.AccountRoleType = this.AccountRoleType;
            objAccount.SecurityQuestionCity = this.SecurityQuestionCity;
            objAccount.SecurityQuestionPhone = this.SecurityQuestionPhone;
            objAccount.SecurityQuestionSchool = this.SecurityQuestionSchool;

            int returnValue = pxy.CreateAccount(objAccount);

            return returnValue;
        }
        public int SecurityQuestions ()
        {
            objAccount.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccount.SecurityQuestionCity = this.SecurityQuestionCity;
            objAccount.SecurityQuestionPhone = this.SecurityQuestionPhone;
            objAccount.SecurityQuestionSchool = this.SecurityQuestionSchool;

            int numOfCorrectResponses = pxy.SecurityQuestions(objAccount);

            return numOfCorrectResponses;
        }
        /// <summary>
        /// Execute the TP_Account_Update_Password_SP stored procedure to update the AccountPassword
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="thePassword"></param>
        /// <returns>Integer number of rows affected by the update, or -1 for an exception</returns>
        public int UpdatePassword()
        {
            objAccount.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccount.AccountPassword = this.AccountPassword;

            int returnValue = pxy.UpdatePassword(objAccount);

            return returnValue;
        }
        /// <summary>
        /// Encrypt the given password and return an Encrypted Byte[]
        /// </summary>
        /// <param name="thePassword"></param>
        /// <returns></returns>
        public static Byte[] Encrypt(string thePassword)
        {
            UTF8Encoding encoder = new UTF8Encoding(); // used to convert bytes to characters, and back
            Byte[] textBytes;                          // stores the plain text data as bytes

            // Perform Encryption
            //-------------------
            // Convert a string to a byte array, which will be used in the encryption process.
            textBytes = encoder.GetBytes(thePassword);

            // Create an instances of the encryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the encrypted data temporarily, and
            // a crypto stream that performs the encryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

            // Use the crypto stream to perform the encryption on the plain text byte array.
            myEncryptionStream.Write(textBytes, 0, textBytes.Length);
            myEncryptionStream.FlushFinalBlock();

            // Retrieve the encrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

            // Close all the streams.
            myEncryptionStream.Close();
            myMemoryStream.Close();

            // Convert the bytes to a string and display it.
            return encryptedBytes;
        }
        private static void Decrypt(string thePassword)
        {
            Byte[] encryptedPasswordBytes = Convert.FromBase64String(thePassword);
            Byte[] textBytes;
            UTF8Encoding encoder = new UTF8Encoding();

            // Perform Decryption
            //-------------------
            // Create an instances of the decryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the decrypted data temporarily, and
            // a crypto stream that performs the decryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myDecryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateDecryptor(key, vector), CryptoStreamMode.Write);

            // Use the crypto stream to perform the decryption on the encrypted data in the byte array.
            myDecryptionStream.Write(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
            myDecryptionStream.FlushFinalBlock();

            // Retrieve the decrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            textBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(textBytes, 0, textBytes.Length);

            // Close all the streams.
            myDecryptionStream.Close();
            myMemoryStream.Close();

            encoder.GetString(textBytes);
        }
        void InitializeThis ()
        {
            this.AccountId = objAccount.AccountId;
            this.UserName = objAccount.UserName;
            this.UserAddress = objAccount.UserAddress;
            this.PhoneNumber = objAccount.PhoneNumber;
            this.CreatedEmailAddress = objAccount.CreatedEmailAddress;
            this.ContactEmailAddress = objAccount.ContactEmailAddress;
            this.Avatar = objAccount.Avatar;
            this.AccountPassword = objAccount.AccountPassword;
            this.Active = objAccount.Active;
            this.DateTimeStamp = objAccount.DateTimeStamp;
            this.AccountRoleType = objAccount.AccountRoleType;
        }

        public int AccountId
        {
            get { return accountId; }
            set { accountId = value;  }
        }
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public String UserAddress
        {
            get { return userAddress; }
            set { userAddress = value; }
        }
        public String PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public String CreatedEmailAddress
        {
            get { return createdEmailAddress; }
            set { createdEmailAddress = value; }
        }
        public String ContactEmailAddress
        {
            get { return contactEmailAddress; }
            set { contactEmailAddress = value; } 
        }
        public int Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        public Byte[] AccountPassword
        {
            get { return accountPassword; }
            set { accountPassword = value; }
        }
        public String Active
        {
            get { return active; }
            set { active = value; }
        }
        public DateTime DateTimeStamp
        {
            get { return dateTimeStamp; }
            set { dateTimeStamp = value; }
        }
        public string AccountRoleType
        {
            get { return accountRoleType; }
            set { accountRoleType = value; }
        }
        public string SecurityQuestionCity
        {
            get { return securityQuestionCity; }
            set { securityQuestionCity = value; }
        }
        public string SecurityQuestionPhone
        {
            get { return securityQuestionPhone; }
            set { securityQuestionPhone = value; }
        }
        public string SecurityQuestionSchool
        {
            get { return securityQuestionSchool; }
            set { securityQuestionSchool = value; }
        }
    }
}
