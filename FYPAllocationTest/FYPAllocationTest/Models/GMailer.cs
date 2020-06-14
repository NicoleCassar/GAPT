using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace FYPAllocationTest.Models
{
    public class GMailer // This model is repsonsible for the sending of emails through the system
    { // For the puposes of this system, Gmail has been chosen as the SMTP client
        public static string GmailUsername { get; set; } // Username for email account
        public static string GmailPassword { get; set; } // Password for email account
        public static string GmailHost { get; set; } // Host address for client
        public static int GmailPort { get; set; } // Port to use with client
        public static bool GmailSSL { get; set; } // Set secure socket layer for mailer
        public string ToEmail { get; set; } // Specify the recipient of the email
        public string Subject { get; set; } // Subject for the email
        public string Body { get; set; } // Template to be used as the email body
        public bool IsHtml { get; set; } // Set email body to obey HTML rules or not

        static GMailer()
        {
            GmailHost = "smtp.gmail.com"; // Setting host
            GmailPort = 587; // Gmail can use ports 25, 465 & 587
            GmailSSL = true; // enabling secure socket layer
        }

        public void Send() // Method for actual sending of emails
        {
            SmtpClient smtp = new SmtpClient(); // Instantiate SMTP lient object
            smtp.Host = GmailHost; // Specify the host
            smtp.Port = GmailPort; // Specify the port
            smtp.EnableSsl = GmailSSL; // Pass secure socket layer settings
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // Specify how emails are delivered
            smtp.UseDefaultCredentials = false; // Set custom credentials for email to be used for mailer
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword); // Get credentials for the email to be used as the sender

            using (var message = new MailMessage(GmailUsername, ToEmail))
            {// Attach FormB onto the email as is done with allocation results each year
                Attachment FormB = new Attachment(Directory.GetCurrentDirectory() + "\\wwwroot\\files\\FYP2021_Form B - SD.docx", MediaTypeNames.Application.Octet);
                message.Subject = Subject; // Set the subject of the email
                message.Body = Body; // Apply a template to the body
                message.IsBodyHtml = IsHtml; // Specify whether HTML rules are to be obeyed
                message.Attachments.Add(FormB); // Add the attachment to the email
                smtp.Send(message); // Send the email
            }
        }
    }
}
