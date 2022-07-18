using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQ_Model
    {
        MessageQueue messageQ = new MessageQueue();
        public void sendData2Queue(string token)
        {
            messageQ.Path = @".\private$\Messages";//Setting the QueuPath where we want to store the messages
            if (!MessageQueue.Exists(messageQ.Path))
            {
                MessageQueue.Create(messageQ.Path);
            }
            messageQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQ.ReceiveCompleted += MessageQ_ReceiveCompleted;
            messageQ.Send(token);
            messageQ.BeginReceive();
            messageQ.Close();
        }

        private void MessageQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = messageQ.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            string subject = "Fundoo Notes Reset Link";
            string Body = token;
            var SMTP = new SmtpClient("Smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sample2dummy@gmail.com", "opnumvngjptzxvnf"),
                EnableSsl = true
            };
            SMTP.Send("sample2dummy@gmail.com", "sample2dummy@gmail.com", subject, Body);
            messageQ.BeginReceive();
        }
    }
}
