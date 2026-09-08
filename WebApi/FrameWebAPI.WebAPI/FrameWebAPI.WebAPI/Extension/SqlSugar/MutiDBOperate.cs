namespace FrameWebAPI.WebAPI.Extension.SqlSugar
{
    public static class MainDb
    {
        public static string CurrentDbConnId = "MainDBConnId";
    }

    public class MutiDBOperate
    {
        public string ConnId { get; set; }

        public string MainConnId { get; set; }

        public bool Enabled { get; set; }

        public int HitRate { get; set; }

        public string Connection { get; set; }

        public DataBaseType DbType { get; set; }
    }

    public enum DataBaseType
    {
        MySql,
        SqlServer,
        Sqlite,
        Oracle,
        PostgreSQL
    }

}
