using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using GameStore.BLL.Interfaces.MailServer;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Infrastructure.MailServer
{
    public class MailNotification : IObserver
    {
        private void SendMessage(Order order, string mail)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            System.Net.NetworkCredential credentials =
            new System.Net.NetworkCredential("katerynakovalenko96@gmail.com", "TestPassword");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage
            {
                From = new MailAddress("katerynakovalenko96@gmail.com")
            };
            msg.To.Add(new MailAddress(mail));

            msg.Subject = "Purchase confirmation";
            msg.IsBodyHtml = true;
            var body = new StringBuilder("<html><head></head><body><section><h3>Hello!</h3><div>We have new order #" +
                order.Id + ":</div><div> Order details:</div><table><tr><td>Game</td><td>Quantity</td><td>Price</td></tr>");
            foreach (var orderDetail in order.OrderDetails)
            {
                body.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                    orderDetail.GameId, orderDetail.Quantity, orderDetail.Price * orderDetail.Quantity);
            }
            body.Append("</table></section></body></html>");
            msg.Body = body.ToString();
            client.Send(msg);
        }

        public void Notify(Order order, IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                SendMessage(order, user.Email);
            }
        }

        public NotificationMethod Method
        {
            get { return NotificationMethod.Mail; }
        }
    }
}

