using SqlSugar;

namespace FrameWebAPI.WebAPI.Extension.SqlSugar
{
    internal class DbLog
    {
        public string Ip { get; set; }

        public string Api { get; set; }

        public string User { get; set; }

        public string Class { get; set; }

        public string Method { get; set; }

        public int Line { get; set; }

        public double ExecutionTime { get; set; }

        public string Sql { get; set; }

        public string Para { get; set; }

        public List<DiffLogTableInfo> BeforeData { get; set; }

        public List<DiffLogTableInfo> AfterData { get; set; }

        public Exception Ex { get; set; }
    }
}
