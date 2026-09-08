using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Service.Extensions
{
    public static class Message
    {
        public static string GetMessage(this string str,int code,string language) 
		{
			string res = str;	
			try
			{

			}
			catch (Exception)
			{
				throw;
			}
			return res;
        }

    }
}
