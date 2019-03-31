using System.Net.Mail;
using NBL.BLL.Contracts;

namespace NBL.BLL
{
    class CrmManager:ICrmManager
    {
        public bool SendMail(MailMessage mailMessage)
        {
            using (var smtp = new SmtpClient())
            {

                smtp.Send(mailMessage);
                return true;
            }
        }
    }
}
