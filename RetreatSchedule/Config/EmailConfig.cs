using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Config
{
    public class EmailConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableTLS { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
