using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace FrameWebAPI.WebAPI.Models.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("user_mstr")]
    public class UserMstr
    {
        /// <summary>
        /// 描述 : 
        /// 允许空值 : False
        /// 默认值 : 
        /// </summary>        
        [SugarColumn(ColumnName = "id", IsNullable = false, IsPrimaryKey = true)]
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 
        /// 允许空值 : True
        /// 默认值 : 
        /// </summary>        
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 描述 : 
        /// 允许空值 : True
        /// 默认值 : 
        /// </summary>        
        [SugarColumn(ColumnName = "addr")]
        public string Addr { get; set; }

    }
}