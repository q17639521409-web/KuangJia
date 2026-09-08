using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;

namespace FrameWebAPI.WebAPI.Extension.SqlSugar
{
    public class BaseDBConfig
    {
        public static (List<MutiDBOperate> allDbs, List<MutiDBOperate> slaveDbs) MutiConnectionString => MutiInitConn();

        private static string DifDBConnOfSecurity(string connFile, string conn)
        {
            if (File.Exists(connFile))
            {
                return File.ReadAllText(connFile).Trim();
            }
            return conn;
        }

        public static (List<MutiDBOperate>, List<MutiDBOperate>) MutiInitConn()
        {
            List<MutiDBOperate> item = (from db in AppSetting.App<MutiDBOperate>(new string[1] { "DBS" })
                                        where db.Enabled && db.MainConnId.GetIsEmptyOrNull()
                                        select db).ToList();
            List<MutiDBOperate> item2 = (from db in AppSetting.App<MutiDBOperate>(new string[1] { "DBS" })
                                         where db.Enabled && db.MainConnId.GetIsNotEmptyOrNull()
                                         select db).ToList();
            return (item, item2);
        }

        private static MutiDBOperate SpecialDbString(MutiDBOperate mutiDBOperate)
        {
            if (mutiDBOperate.DbType == DataBaseType.Sqlite)
            {
                mutiDBOperate.Connection = "DataSource=" + Path.Combine(Environment.CurrentDirectory, mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.SqlServer)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity("D:\\my-file\\dbCountPsw1_SqlserverConn.txt", mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.MySql)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity("D:\\my-file\\dbCountPsw1_MySqlConn.txt", mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.Oracle)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity("D:\\my-file\\dbCountPsw1_OracleConn.txt", mutiDBOperate.Connection);
            }
            return mutiDBOperate;
        }
    }
}
