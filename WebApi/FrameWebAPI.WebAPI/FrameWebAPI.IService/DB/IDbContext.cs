using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.IService.DB
{
    public interface IDbContext<TEntity> where TEntity : class, new()
    {
        ISqlSugarClient Db { get; }

        //void ChangeDB(string configId);
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Query();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> Query(int pageIndex, int pageSize, int total);
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        /// <summary>
        /// 分页条件查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>

        Task<int> Insertable(List<TEntity> list);

        /// <summary>
        /// 插入数据
        /// </summary>
        Task<bool> Insert(TEntity entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        Task<bool> Delete(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        Task<bool> Delete(List<TEntity> Model);
    }
}
