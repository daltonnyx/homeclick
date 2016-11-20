using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Homeclick.Models
{
    public partial class Feedback
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string Name { get; set; }

        public string Organisation { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }

    public partial class Feedback
    {
        public void Send(string to)
        {
            string server = "";

            var from = this.Email;
            var subject = "Feedback";

            if (this.Subject != null)
                subject = this.Subject;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Người gửi: " + this.Name);

            if (this.Organisation != null)
                sb.AppendLine("Công ty: " + this.Organisation);

            if (this.Phone != null)
                sb.AppendLine("Số điện thoại: " + this.Phone);

            sb.AppendLine("Nội dung: " + this.Message);

            var content = sb.ToString();

            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = content;
            SmtpClient client = new SmtpClient(server);

            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.
            client.UseDefaultCredentials = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                            ex.ToString());
            }
        }
    }
}