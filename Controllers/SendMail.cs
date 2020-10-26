using System;
using System.Collections.Generic;

using MimeKit;
using System.Text;
using Google.Apis.Gmail.v1.Data;
using Gmail_Api.Model;

/// <summary>
/// Summary description for SendMail
/// </summary>
/// 
namespace Gmail_Api.Controllers
{
    public class SendMail
    {
        public Message CreateMessage(mMessage mmessage) 
        {
            List<String> to = mmessage.to;
            string from = mmessage.from;
            List<string> cc = mmessage.cc;
            string subject = mmessage.subject;
            string plainTextMessage = mmessage.plainTextMessage;
            string htmlMessage = mmessage.htmlMessage;
            string replyTo = null;


            var m = new MimeMessage();

            this.checkWhiteSpaceOrNull(subject, "subject");
            m.Subject = subject;

            this.checkWhiteSpaceOrNull(from, "from");
            m.From.Add(new MailboxAddress("", from));

            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                m.ReplyTo.Add(new MailboxAddress("", replyTo));
            }

            to.ForEach(delegate (string eachTo)
            {
                this.checkWhiteSpaceOrNull(eachTo, "to");
                m.To.Add(new MailboxAddress("", eachTo));
            });

            cc.ForEach(delegate (string eachCc)
            {
                this.checkWhiteSpaceOrNull(eachCc, "cc");
                m.Cc.Add(new MailboxAddress("", eachCc));
            });

            this.checkWhiteSpaceOrNull(htmlMessage, "htmlMessage");
            BodyBuilder bodyBuilder = this.GetBodyBuilder(htmlMessage);

            if (!string.IsNullOrWhiteSpace(plainTextMessage))
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            m.Body = bodyBuilder.ToMessageBody();

            var message = new Message
            {
                Raw = Encode(m.ToString())
            };

            return message;
        }

        private string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

        private BodyBuilder GetBodyBuilder(string htmlMsg)
        {
            BodyBuilder builder = new BodyBuilder();
           // var logo = builder.LinkedResources.Add(Path.Combine(HttpRuntime.AppDomainAppPath, "img/logo-250.png"));
           // logo.ContentId = MimeUtils.GenerateMessageId();
            StringBuilder sb = new StringBuilder();
            //sb.Append("<div style='font-size:1.15em;color:#222;padding:30px;max-width:600px;margin:auto;border-radius:5px;border:1px solid #d3d5d6'><div style='text-align:center'>");
            ////sb.Append(string.Format(@"<img style='width:200px' src='cid:{0}'/>", logo.ContentId));
            //sb.Append("<h1 style='font-weight:500;color:#222;margin:0;font-size:1.2em'>IDP Education (Cambodia)</h1><h2 style='font-weight:500;color:#222;font-size:1em;margin:0'>Australian Centre for Education</h2></div><hr style='border:1px solid #aba8a8;margin:20px auto;max-width:100px'/>");
            sb.Append(htmlMsg).Append("</div>");

            builder.HtmlBody = sb.ToString();
            return builder;
        }

        private void checkWhiteSpaceOrNull(string str, string displayString)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("no " + displayString + " address provided");
            }
        }
    }
}