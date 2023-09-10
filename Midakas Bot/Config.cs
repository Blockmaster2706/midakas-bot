using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midakas_Bot
{
    public class Config
    {
        public Config() 
        {
            TOKEN = "";
            GUILDID = "";
        }

        public string TOKEN { get; set; }
        public string GUILDID { get; set; }
    }
}
