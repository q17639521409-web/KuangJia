using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Common.HttpContext
{
    public interface IHttpContextCore
    {
        string IP { get; }

        string API { get; }

        string User { get; }
    }

}
