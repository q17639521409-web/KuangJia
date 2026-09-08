using FrameWebAPI.IService.DB;
using FrameWebAPI.Model.Model;
using SqlSugar;
using FrameWebAPI.Service.Helper;
using FrameWebAPI.Service.Extensions;
using System.Linq.Expressions;

namespace FrameWebAPI.Service.DB
{
    public class DbContext<TEntity> : IDbContext<TEntity> where TEntity : class, new()
    {
        private readonly SqlSugarClient _BaseDal;

        private readonly IUnitOfWork _unitOfWork;

        private bool isChangeDb;

        public bool _multiTask;
        public ISqlSugarClient Db
        {
            get
            {
                if (!isChangeDb && typeof(TEntity).GetTypeInfo().GetCustomAttributes(typeof(SugarTable), inherit: true).FirstOrDefault((object x) => x.GetType() == typeof(SugarTable)) is SugarTable sugarTable && !string.IsNullOrEmpty(sugarTable.TableDescription))
                {
                    _BaseDal.ChangeDatabase(sugarTable.TableDescription.ToLower());
                }
                if (_multiTask)
                {
                    return _BaseDal.CopyNew();
                }
                return _BaseDal;
            }
        }
     
        public DbContext()
        {

        }

        public DbContext(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _BaseDal = unitOfWork.GetDbClient();
        }

        #region 暂无用-每次新建示例（简单项目使用）
        //public DbContext()
        //{
        //    try
        //    {
        //        var mainDbId = AppSetting.App("MainDB").GetCString();
        //        var dbList = AppSetting.App<DatabaseConfig>("DBS");

        //        var mainDbConfig = dbList.FirstOrDefault(p => p.ConnId == mainDbId);
        //        if (mainDbConfig == null)
        //        {
        //            SerilogHelper.WriteLog("SqlSugarHelper", "OnLogExecuting", $"Main database configuration not found.");
        //            return;
        //        }

        //        _BaseDal = new SqlSugarClient(new ConnectionConfig()
        //        {
        //            ConnectionString = mainDbConfig.Connection,
        //            DbType = (DbType)mainDbConfig.DBType,
        //            InitKeyType = InitKeyType.Attribute,
        //            IsAutoCloseConnection = true,
        //        });

        //        foreach (var dbConfig in dbList.Where(p => p.ConnId != mainDbId))
        //        {
        //            _BaseDal.AddConnection(new ConnectionConfig()
        //            {
        //                ConnectionString = dbConfig.Connection,
        //                DbType = (DbType)dbConfig.DBType,
        //                IsAutoCloseConnection = true,
        //                ConfigId = dbConfig.ConnId
        //            });
        //        }

        //        if (AppSetting.App("EnableSqlLog").GetCBool())
        //        {
        //            _BaseDal.Aop.OnLogExecuting = (sql, pars) =>
        //            {
        //                SerilogHelper.WriteLog("SqlSugarHelper", "OnLogExecuting", $"sql:{sql}");
        //                SerilogHelper.WriteLog("SqlSugarHelper", "OnLogExecuting", $"pars:{JsonHelper.ObjectToJson(pars)}");
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SerilogHelper.WriteLog("DbContext", "Constructor", $"Failed to initialize database connection: {ex.Message}");
        //        throw new Exception("Failed to initialize database connection.", ex);
        //    }
        //}

        //public DbContext(DatabaseConfig databaseConfig)
        //{
        //    try
        //    {
        //        _BaseDal = new SqlSugarClient(new ConnectionConfig()
        //        {
        //            ConnectionString = databaseConfig.Connection,
        //            DbType = (DbType)databaseConfig.DBType,
        //            InitKeyType = InitKeyType.Attribute,
        //            IsAutoCloseConnection = true,
        //        });

        //        if (AppSetting.App("EnableSqlLog").GetCBool())
        //        {
        //            _BaseDal.Aop.OnLogExecuting = (sql, pars) =>
        //            {
        //                SerilogHelper.WriteLog("SqlSugarHelper", "OnLogExecuting", $"sql:{sql}");
        //                SerilogHelper.WriteLog("SqlSugarHelper", "OnLogExecuting", $"pars:{JsonHelper.ObjectToJson(pars)}");
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SerilogHelper.WriteLog("DbContext", "Constructor", $"Failed to initialize database connection: {ex.Message}");
        //        throw new Exception("Failed to initialize database connection.", ex);
        //    }
        //}
        #endregion

        public void ChangeDatabase(string configId)
        {
            if (_BaseDal != null)
            {
                _BaseDal.ChangeDatabase(configId);
                isChangeDb = true;
            }
            else
            {
                throw new InvalidOperationException("Database client is not initialized.");
            }
        }

        public async Task<List<TEntity>> Query()
        {
            return await _BaseDal.Queryable<TEntity>().ToListAsync();
        }
         
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _BaseDal.Queryable<TEntity>().Where(whereExpression).ToListAsync();
        }

        public async Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize)
        {
            return await _BaseDal.Queryable<TEntity>()
                .Where(whereExpression)
                .ToPageListAsync(pageIndex, pageSize);
        }

        public async Task<bool> Insert(TEntity entity)
        {
            return await _BaseDal.Insertable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Update(TEntity entity)
        {
            return await _BaseDal.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _BaseDal.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandAsync() > 0;
        }
        public async Task<bool> Delete(List<TEntity> List)
        {
            return await _BaseDal.Deleteable<TEntity>(List).ExecuteCommandAsync() > 0;
        }

        public async Task<List<TEntity>> Query(int pageIndex, int pageSize, int total)
        {
            return await _BaseDal.Queryable<TEntity>().ToPageListAsync(pageIndex, pageSize, total);
        }

        public async Task<int> Insertable(List<TEntity> list)
        {
            return await _BaseDal.Insertable(list).ExecuteCommandAsync();
        }

    }
}
