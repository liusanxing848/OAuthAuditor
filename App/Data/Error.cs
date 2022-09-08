using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.Data
{
    class Error
    {
        public static StringBuilder sb = new StringBuilder();
        public static string ErrorLog ()
        {
            return sb.ToString();
        }

    }
}
