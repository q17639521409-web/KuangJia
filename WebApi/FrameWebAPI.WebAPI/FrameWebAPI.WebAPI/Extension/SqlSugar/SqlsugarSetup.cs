using FrameWebAPI.Common.HttpContext;
using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;
using SqlSugar;

namespace FrameWebAPI.WebAPI.Extension.SqlSugar
{
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            //ICacheService myCache = new SqlSugarRedisCache();
            if (AppSetting.App("AppSetting", "DBAOP", "Enabled").GetCBool())
            {
                List<string> hostNames = AppSetting.App<string>(new string[4] { "AppSetting", "DBAOP", "RabbitMQ", "HostNames" });
                string userName = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "UserName");
                string password = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "Password");
                int cInt = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "Port").GetCInt();
                string virtualHost = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "VirtualHost");
                string exchangeName = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "ExchangeName");
                string queueName = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "QueueName");
                string routeKey = AppSetting.App("AppSetting", "DBAOP", "RabbitMQ", "RouteKey");
                //rabbitHelper = new RabbitHelper(hostNames, userName, password, cInt, virtualHost, exchangeName, queueName, routeKey);
            }
            services.AddScoped((Func<IServiceProvider, ISqlSugarClient>)delegate
            {
                MainDb.CurrentDbConnId = AppSetting.App("MainDB");
                List<ConnectionConfig> listConfig = new List<ConnectionConfig>();
                BaseDBConfig.MutiConnectionString.allDbs.ForEach(delegate (MutiDBOperate m)
                {
                    List<SlaveConnectionConfig> listConfig_Slave = new List<SlaveConnectionConfig>();
                    BaseDBConfig.MutiConnectionString.slaveDbs.ForEach(delegate (MutiDBOperate s)
                    {
                        if (s.MainConnId.GetCString().ToLower() == m.ConnId.GetCString().ToLower())
                        {
                            listConfig_Slave.Add(new SlaveConnectionConfig
                            {
                                HitRate = s.HitRate,
                                ConnectionString = s.Connection
                            });
                        }
                    });
                    listConfig.Add(new ConnectionConfig
                    {
                        ConfigId = m.ConnId.GetCString().ToLower(),
                        ConnectionString = m.Connection,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,
                        AopEvents = new AopEvents(),
                        MoreSettings = new ConnMoreSettings
                        {
                            IsAutoRemoveDataCache = true,
                            PgSqlIsAutoToLower = true
                        },
                        SlaveConnectionConfigs = listConfig_Slave,//表示该主数据库的从数据库配置
                        ConfigureExternalServices = new ConfigureExternalServices
                        {
                            //DataInfoCacheService = (AppSetting.App("Startup", "SqlSugar", "RedisEnabled").GetCBool() ? myCache : null),
                            AppendDataReaderTypeMappings = new List<KeyValuePair<string, CSharpDataType>>
                            {
                                new KeyValuePair<string, CSharpDataType>("varchar", CSharpDataType.@string),
                                new KeyValuePair<string, CSharpDataType>("tid", CSharpDataType.@object),
                                new KeyValuePair<string, CSharpDataType>("public.citext", CSharpDataType.@string)
                            }
                        },
                        InitKeyType = InitKeyType.Attribute
                    });
                });
                SqlSugarClient db = new SqlSugarClient(listConfig);
                if (AppSetting.App("AppSetting", "SqlAOP", "Enabled").GetCBool())
                {

                    db.Aop.OnLogExecuted = delegate (string sql, SugarParameter[] p)
                    {
                        _ = db.Ado.SqlStackTrace.FirstFileName;
                        _ = db.Ado.SqlStackTrace.FirstLine;
                        _ = db.Ado.SqlStackTrace.FirstMethodName;
                        _ = db.Ado.SqlStackTrace.MyStackTraceList;
                        Parallel.For(0, 1, (Action<int>)delegate
                        {
                            DbLog obj3 = new DbLog
                            {
                                Ip = StaticHttpContext.IP,
                                Api = StaticHttpContext.API,
                                User = StaticHttpContext.User,
                                Class = db.Ado.SqlStackTrace.FirstFileName,
                                Method = db.Ado.SqlStackTrace.FirstMethodName,
                                Line = db.Ado.SqlStackTrace.FirstLine,
                                Sql = sql,
                                Para = JsonHelper.ObjectToJson(p),
                                ExecutionTime = db.Ado.SqlExecutionTime.TotalMilliseconds
                            };
                            SerilogHelper.WriteLog("SqlLog", "ExecutLog", JsonHelper.JsonFormat(JsonHelper.ObjectToJson(obj3)));
                        });
                    };
                    db.Aop.OnError = delegate (SqlSugarException ex)
                    {
                        Parallel.For(0, 1, (Action<int>)delegate
                        {
                            DbLog obj2 = new DbLog
                            {
                                Ip = StaticHttpContext.IP,
                                Api = StaticHttpContext.API,
                                User = StaticHttpContext.User,
                                Class = db.Ado.SqlStackTrace.FirstFileName,
                                Method = db.Ado.SqlStackTrace.FirstMethodName,
                                Line = db.Ado.SqlStackTrace.FirstLine,
                                Sql = db.Ado.SqlParameterKeyWord,
                                ExecutionTime = db.Ado.SqlExecutionTime.TotalMilliseconds,
                                Ex = ex
                            };
                            SerilogHelper.WriteLog("SqlLog", "ErrorLog", JsonHelper.JsonFormat(JsonHelper.ObjectToJson(obj2)));
                        });
                    };
                    db.Aop.OnDiffLogEvent = delegate (DiffLogModel DiffLogModel)
                    {
                        Parallel.For(0, 1, (Action<int>)delegate
                        {
                            DbLog obj = new DbLog
                            {
                                Ip = StaticHttpContext.IP,
                                Api = StaticHttpContext.API,
                                User = StaticHttpContext.User,
                                Class = db.Ado.SqlStackTrace.FirstFileName,
                                Method = db.Ado.SqlStackTrace.FirstMethodName,
                                Line = db.Ado.SqlStackTrace.FirstLine,
                                Sql = db.Ado.SqlParameterKeyWord,
                                ExecutionTime = db.Ado.SqlExecutionTime.TotalMilliseconds,
                                BeforeData = DiffLogModel.BeforeData,
                                AfterData = DiffLogModel.AfterData
                            };
                            //SendDbLog(DiffLogModel, db.Ado.Connection.ConnectionString, db.Ado.Connection.Database);
                            SerilogHelper.WriteLog("SqlLog", "DiffLog", JsonHelper.JsonFormat(JsonHelper.ObjectToJson(obj)));
                        });
                    };
                }
                return db;
            });
        }
    }
}