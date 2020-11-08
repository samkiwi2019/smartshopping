using System;

namespace Smartshopping.Library
{
    public static class MyUtils
    {
        public static string ExceptionMessage(Exception ex)
        {
            string message;

            if (ex.Message== "An error occurred while updating the entries. See the inner exception for details."
                && ex.InnerException!=null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                message = ex.InnerException.Message;
            }
            else
            {
                message = ex.Message;
            }

            return message;
        }
    }
}