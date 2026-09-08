using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Service.Helper
{
    public class SerilogHelper
    {
        static StringBuilder sb = new StringBuilder();
        public static void WriteLog(string name, string methodName, params string[] dataParas)
        {
            sb.Clear();
            string filename = name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            var logText = "log/";
            string fileDic = AppDomain.CurrentDomain.BaseDirectory + logText + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(fileDic))
            {
                Directory.CreateDirectory(fileDic);
            }

            string filePath = fileDic + "/" + filename;
            FileInfo file = new FileInfo(filePath);

            sb.Append(DateTime.Now.ToString() + "--");
            sb.Append(name + " -- ");
            sb.Append(methodName + " : ");
            for (int i = 0; i < dataParas.Length; i++)
            {
                string text2 = dataParas[i];
                sb.Append("\r\n" + text2 + "\r\n");

            }
            sb.Append("Current ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "\r\n");

            FileMode fm = new FileMode();
            if (!file.Exists)
            {
                fm = FileMode.Create;
            }
            else
            {
                fm = FileMode.Append;
            }
            using (FileStream fs = new FileStream(filePath, fm, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }
            }
        }

        public static void WriteErrorLog(string name, string methodName, Exception ex)
        {
            string[] dataParas = new string[1];
            dataParas[0] = ex.Message;

            sb.Clear();
            string filename = name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            var logText = "log/";
            string fileDic = AppDomain.CurrentDomain.BaseDirectory + logText + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(fileDic))
            {
                Directory.CreateDirectory(fileDic);
            }

            string filePath = fileDic + "/" + filename;
            FileInfo file = new FileInfo(filePath);

            sb.Append(DateTime.Now.ToString() + "--");
            sb.Append(name + " -- ");
            sb.Append(methodName + " : ");
            for (int i = 0; i < dataParas.Length; i++)
            {
                string text2 = dataParas[i];
                sb.Append("\r\n" + text2 + "\r\n");

            }
            sb.Append("Current ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "\r\n");

            FileMode fm = new FileMode();
            if (!file.Exists)
            {
                fm = FileMode.Create;
            }
            else
            {
                fm = FileMode.Append;
            }
            using (FileStream fs = new FileStream(filePath, fm, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }
            }
        }
    }
}
