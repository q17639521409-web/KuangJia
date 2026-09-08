using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Model.Model
{
    public class DatabaseConfig
    {
        public string ConnId { get; set; }
        public int DBType { get; set; }
        public bool Enabled { get; set; }
        public int HitRate { get; set; }
        public string Connection { get; set; }
    }
}
