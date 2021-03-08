using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmpManagmentWebApi.Models
{
    public class CommanFunction
    {
        public static int SendConfirmationLink(string confirmationLInk, string name, string email, string Ref)
        {
            int result = 0;
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(email);
            if (Ref == "NewAccountConfirmation")
            {
                mailMessage.Subject = "Registration successful";
            }
            else
            {
                mailMessage.Subject = "Reset Password";
            }
            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("Hello " + name + ",");
            if (Ref == "NewAccountConfirmation")
            {
                sbEmailBody.Append("<br /><br />Before you can Login, please confirm your email, by clicking on the confirmation link we have emailed you <b>");
                sbEmailBody.Append("<br /><br />Click <a href='" + confirmationLInk + "'>here</a> to confirmation your account.");
            }
            else
            {
                sbEmailBody.Append("<br /><br />Reset password link emaild you on your registerd email id , please login to your email account, and click on the rest link we have emailed you <b>");
                sbEmailBody.Append("<br /><br />Click <a href='" + confirmationLInk + "'>here</a> to confirmation your account.");
            }
            sbEmailBody.Append("<br /><br />Thanks,");
            sbEmailBody.Append("<br /><br />" + Startup.StaticConfig["AppName"]);
            mailMessage.Body = sbEmailBody.ToString();
            result = SendEmail(mailMessage);
            return result;
        }
        //public bool SendEmail(string sEmail, string sName, string sSubject, string sMessage)
        //{
        //    bool bStatus = false;
        //    sSubject = "New user registration successful.";
        //    MailMessage objMail = new MailMessage();
        //    objMail = AddEmailToAddress(objMail, sEmail);
        //    objMail.Subject = "MLM Enquiry Regarding" + "  " + sSubject;
        //    objMail.Body = sMessage;
        //    objMail.Body = sMessage.ToString();
        //    objMail.IsBodyHtml = true;
        //    int icheckStatus = SendEmail(objMail);
        //    if (icheckStatus == 1)
        //    {
        //        bStatus = true;
        //    }
        //    return bStatus;
        //}

        public static int SendEmail(MailMessage mailMessage)
        {
            int status = 0;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = Startup.StaticConfig["Host"];
            smtp.Port = Convert.ToInt32(Startup.StaticConfig["EmailPort"]);
            smtp.Credentials = new NetworkCredential(Startup.StaticConfig["SenderEmail"], Startup.StaticConfig["SenderEmailPassword"]);
            mailMessage.Sender = new MailAddress(Startup.StaticConfig["SenderEmail"], Startup.StaticConfig["AppName"]);
            mailMessage.From = new MailAddress(Startup.StaticConfig["SenderEmail"], Startup.StaticConfig["AppName"]);
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            smtp.EnableSsl = Convert.ToBoolean(Startup.StaticConfig["EnableSsl"]);
            mailMessage.IsBodyHtml = true;
            smtp.Send(mailMessage);
            status = 1;
            return status;
        }
        public MailMessage AddEmailToAddress(MailMessage objMail, string email)
        {
            string[] StrEmailTo = email.Split(';');

            if (StrEmailTo.Count() > 0)
            {
                foreach (var item in StrEmailTo)
                {
                    if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item) && ValidateEmail(item))
                    {
                        objMail.To.Add(item);
                    }
                }
            }
            return objMail;
        }
        public static bool ValidateEmail(string strEmail)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(strEmail);
            return match.Success;
        }
    }
}
