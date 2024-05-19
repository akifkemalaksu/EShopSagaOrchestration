using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    public class RabbitMqSettings
    {
        public required string Host { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
