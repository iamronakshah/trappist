using System;

namespace Promact.Trappist.Utility.Constants
{
    public class StringConstants : IStringConstants
    {


        public string InvalidTestName
        {
            get
            {
                return "Invalid Test Name ";
            }
        }

        public string Success
        {
            get
            {
                return "Test Created successfuly";
            }
        }

        public string CharactersForLink
        {
            get
            {
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            }
        }

        #region "Account Constants"

        public string InavalidLoginError
        {
            get
            {
                return "Username or Password Is Invalid!";
            }
        }

        public string InavalidModelError
        {
            get
            {
                return "Invalid Login Attempt!";
            }
        }


        #endregion
    }
}
