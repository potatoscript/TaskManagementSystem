using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement
{
    public class MailSettings
    {
        public string Mail { get; set;}
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }


        public IConfiguration _mail;

        public MailSettings(IConfiguration configuration)
        {
            _mail = configuration;

            Mail = String.Format(_mail.GetSection("MailSettings").GetSection("Mail").Value);
            DisplayName = String.Format(_mail.GetSection("MailSettings").GetSection("DisplayName").Value);
            Password = String.Format(_mail.GetSection("MailSettings").GetSection("Pw").Value);
            Host = String.Format(_mail.GetSection("MailSettings").GetSection("Host").Value);
            Port = int.Parse(String.Format(_mail.GetSection("MailSettings").GetSection("Port").Value));

        }

    }
}
