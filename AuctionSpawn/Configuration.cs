using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AuctionSpawn
{
    public class Configuration
    {
        public static string SmtpHost => ConfigurationManager.AppSettings["SmtpHost"];
        public static int  SmtpPort => Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
        public static bool SmtpEnableSsl => Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
        public static bool SmtpUseDefaultCredentials => Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpUseDefaultCredentials"]);
        public static string SmtpDeliveryMethod => ConfigurationManager.AppSettings["SmtpDeliveryMethod"];
        public static string Username => ConfigurationManager.AppSettings["Username"];
        public static string Pass => ConfigurationManager.AppSettings["Pass"];

        public static string ToEmailAddress => ConfigurationManager.AppSettings["ToEmailAddress"];
        public static string FromEmailAddress => ConfigurationManager.AppSettings["FromEmailAddress"];
        public static string EmailSubject => ConfigurationManager.AppSettings["EmailSubject"];

        //Not Tested
        public static int ItemQuantity =>Convert.ToInt32(ConfigurationManager.AppSettings["ItemQuantity"]);

    }
}