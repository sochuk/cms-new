using CMS.Helper;
using CMS.Management;
using CMS.Management.Model;
using CMS.Management.Structure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CMS.Notification
{
    public class Mail
    {
        public static bool Send(string DisplayName, string Title, string Message, MailAddress To, MailAddress CC = null)
        {
            M_Config config = new M_Config();

            MailAddress From = new MailAddress(config.smtp_mail, DisplayName);
            using (MailMessage mail = new MailMessage())
            {
                //Setting From , To and CC
                mail.From = From;
                mail.To.Add(To);
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Subject = Title;
                mail.Body = Message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (CC != null) mail.CC.Add(CC);

                using (var smtpClient = new SmtpClient(config.smtp_server, config.smtp_port))
                {
                    var pwd = Crypto.Decrypt(config.smtp_password, Configuration.key);

                    smtpClient.Credentials = new NetworkCredential(config.smtp_mail, pwd);
                    smtpClient.EnableSsl = true;
                    try
                    {
                        smtpClient.Send(mail);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool Send(MailAddress From, string Title, string Message, MailAddress To, MailAddress CC = null)
        {
            M_Config config = new M_Config();
            using (MailMessage mail = new MailMessage())
            {
                //Setting From , To and CC
                mail.From = From;
                mail.To.Add(To);
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Subject = Title;
                mail.Body = Message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (CC != null) mail.CC.Add(CC);

                using (var smtpClient = new SmtpClient(config.smtp_server, config.smtp_port))
                {
                    smtpClient.Credentials = new NetworkCredential(config.smtp_mail, Crypto.Decrypt(config.smtp_password, Configuration.key));
                    smtpClient.EnableSsl = true;
                    try
                    {
                        smtpClient.Send(mail);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public static bool SendAsync(string DisplayName, string Title, string Message, MailAddress To, MailAddress CC = null)
        {
            M_Config config = new M_Config();
            MailAddress From = new MailAddress(config.smtp_mail, DisplayName);
            using (MailMessage mail = new MailMessage())
            {
                //Setting From , To and CC
                mail.From = From;
                mail.To.Add(To);
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Subject = Title;
                mail.Body = Message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (CC != null) mail.CC.Add(CC);

                using (var smtpClient = new SmtpClient(config.smtp_server, config.smtp_port))
                {
                    smtpClient.Credentials = new NetworkCredential(config.smtp_mail, Crypto.Decrypt(config.smtp_password, Configuration.key));
                    smtpClient.EnableSsl = true;
                    try
                    {
                        smtpClient.SendAsync(mail, "");
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}