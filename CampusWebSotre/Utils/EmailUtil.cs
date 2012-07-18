using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CampusWebStore.Utils
{
    public class EmailUtil
    {
        #region "Properties"

        public string Host { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }

        public string FromName { get; set; }
        
        public string FromAddress { get; set; }
        
        public string HostPortString { get; set; }

        #endregion

        /// <summary>
        /// Constructor...
        /// </summary>
        public EmailUtil()
        {
            //GET THE VALUES FOR SENDING MAIL
            Host = ConfigurationManager.AppSettings["SMTP_SERVER"];

            SmtpUserName = ConfigurationManager.AppSettings["SMTP_USERNAME"];

            SmtpPassword = ConfigurationManager.AppSettings["SMTP_PASSWORD"];

            FromName = ConfigurationManager.AppSettings["FROM_NAME"];

            FromAddress = ConfigurationManager.AppSettings["FROM_ADDRESS"];

            HostPortString = ConfigurationManager.AppSettings["SMTP_PORT"];
        }

        #region "Methods"

        public bool Send(string to,string subject, string msgbody)
        {

            int hostport = Convert.ToInt16(HostPortString);

            //SEND THE MAIL TO THE USER
            var smpt = new SmtpClient
                           {
                               UseDefaultCredentials = false,
                               Credentials = new System.Net.NetworkCredential(SmtpUserName, SmtpPassword),
                               Port = hostport,
                               Host = Host
                           };

            var msg = new MailMessage
                          {
                              Body = msgbody,
                              IsBodyHtml = true,
                              From = new MailAddress(FromAddress, FromName),
                              Subject = subject
                          };

            try
            {
                msg.To.Add(to);

                smpt.Send(msg);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

       

       
    }
}