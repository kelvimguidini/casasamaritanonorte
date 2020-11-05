using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace casasamaritanonorte.Lib
{
    public class EnviarNotificacoes
    {
        public void EnviarEmail(string body, string para, string assunto )
        {
            
            var message = new MailMessage();
            message.To.Add(new MailAddress(para));  // replace with valid value 
            message.From = new MailAddress("kelvimguimg@gmail.com");  // replace with valid value
            message.Subject = assunto;
            message.Body = string.Format(body, "Sistema de gestão casa de repouso");
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;
                
                var credential = new NetworkCredential
                {
                    //UserName = "kelvimguimg@gmail.com",  // replace with valid value
                    //Password = ""  // replace with valid value

                    UserName = "kelvimguimg@gmail.com",  // replace with valid value
                    Password = "Simecao06"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }


  
    }
}