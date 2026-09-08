using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Model.Basic
{
    public class DocResultModel
    {
        public string fileId { get; set; }
        public string mongoDbId { get; set; }
        public string fileName { get; set; }
        public string base64 { get; set; }
    }

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string ResultCode { get; set; } = "100000";
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResultMsg { get; set; } = "执行成功";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
    }

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class ResultModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string ResultCode { get; set; } = "100000";
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResultMsg { get; set; } = "执行成功";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
    }


    /// <summary>
    /// 通用返回信息类（接口响应文档使用）
    /// </summary>
    public class ReturnResultModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string ResultCode { get; set; } = "100000";
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResultMsg { get; set; } = "执行成功";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
    }
    /// <summary>
    /// 通用分页信息类（接口响应文档使用）
    /// </summary>
    public class ReturnPageModel<T>
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int Count { get; set; } = 0;
        /// <summary>
        /// 当前页标
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; } = 100;

    }

}
