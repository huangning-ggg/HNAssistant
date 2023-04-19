using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace HNAssistant
{

    public class EmailAssistant
    {
        public static void SendMailLocalhost()
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            /* 发送 
            * msg.To.Add("b@b.com");  
            * msg.To.Add("b@b.com");可以发送给多人  
            */
            msg.To.Add("1028224015@qq.com");
            //msg.To.Add("b@b.com");

            /* 抄送
            * msg.CC.Add("c@c.com");  
            * msg.CC.Add("c@c.com");可以抄送给多人  
            */
            msg.From = new MailAddress("a@a.com", "AlphaWu", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = "这是测试邮件";//邮件标题  
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码  
            msg.Body = "邮件内容";//邮件内容  
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码  
            msg.IsBodyHtml = false;//是否是HTML邮件  
            msg.Priority = MailPriority.High;//邮件优先级 

            SmtpClient client = new SmtpClient();
            client.Host = "localhost";
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //简单一点儿可以client.Send(msg);  
                MessageBox.Show("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                MessageBox.Show(ex.Message, "发送邮件出错");
            }
        }

        public static void SendMailUseZj()
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add("1028224015@qq.com");
            /*   
            * msg.To.Add("b@b.com");   
            * msg.To.Add("b@b.com");   
            * msg.To.Add("b@b.com");可以发送给多人   
            */
            //msg.CC.Add("c@c.com");
            /*   
            * msg.CC.Add("c@c.com");   
            * msg.CC.Add("c@c.com");可以抄送给多人   
            */
            msg.From = new MailAddress("master@boys90.com", "dulei", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = "这是测试邮件";//邮件标题    
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码    
            msg.Body = "邮件内容";//邮件内容    
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码    
            msg.IsBodyHtml = false;//是否是HTML邮件    
            msg.Priority = MailPriority.High;//邮件优先级    

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("sont.mes@sont-tech.com", "SontMes2017");
            //在71info.com注册的邮箱和密码    
            client.Host = "smtp.exmail.qq.com";
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //简单一点儿可以client.Send(msg);    
                MessageBox.Show("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                MessageBox.Show(ex.Message, "发送邮件出错");
            }
        }

        public static void SendMailUseGmail()
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add("1028224015@qq.com");

            //msg.To.Add("b@b.com");收件人

            //msg.CC.Add("c@c.com");抄送

            msg.From = new MailAddress("sont.mes@sont-tech.com", "test", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = "这是测试邮件";//邮件标题    
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码    
            msg.Body = "邮件内容";//邮件内容    
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码    
            msg.IsBodyHtml = false;//是否是HTML邮件    
            msg.Priority = MailPriority.High;//邮件优先级

            //邮件服务器 qq邮箱是smtp.qq.com,163邮箱是smtp.163.com
            SmtpClient client = new SmtpClient("smtp.exmail.qq.com");
            client.Credentials = new System.Net.NetworkCredential("sont.mes@sont-tech.com", "SontMes2017");
            //上述写你的GMail邮箱和密码    
            client.Port = 587;//Gmail使用的端口    
            
            client.EnableSsl = true;//经过ssl加密    
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //简单一点儿可以client.Send(msg);    
                MessageBox.Show("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                MessageBox.Show(ex.Message, "发送邮件出错");
            }
        }
    }
}
