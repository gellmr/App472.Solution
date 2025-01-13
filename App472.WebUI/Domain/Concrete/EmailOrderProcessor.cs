using System;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;
using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;

namespace App472.WebUI.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = ConfigurationManager.AppSettings["SMTP_MailToAddress"];
        public string MailFromAddress = ConfigurationManager.AppSettings["SMTP_MailFromAddress"];
        public bool UseSsl = Boolean.Parse(ConfigurationManager.AppSettings["SMTP_UseSsl"] ?? "true");
        public string Username = ConfigurationManager.AppSettings["SMTP_Username"];
        public string Password = ConfigurationManager.AppSettings["SMTP_Password"];
        public string ServerName = ConfigurationManager.AppSettings["SMTP_ServerName"];
        public int ServerPort = Int32.Parse(ConfigurationManager.AppSettings["SMTP_ServerPort"]);
        public bool WriteAsFile = Boolean.Parse(ConfigurationManager.AppSettings["SMTP_WriteAsFile"] ?? "false");
        public string FileLocation = ConfigurationManager.AppSettings["SMTP_FileLocation"];
    }

    // Deal with orders by emailing them to an inbox.
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settingsArg)
        {
            emailSettings = settingsArg;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
                }
                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.FirstName)
                    .AppendLine(shippingInfo.LastName)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap:{0}", shippingInfo.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // From
                    emailSettings.MailToAddress,   // To
                    "New order submitted",         // Subject
                    body.ToString());              // Body
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
