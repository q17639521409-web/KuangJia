using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FrameWebAPI.Model.Basic;
using FrameWebAPI.Model.Model;
using Microsoft.AspNetCore.Http;
using FrameWebAPI.WebAPI.Models.Models;

namespace FrameWebAPI.IService.Master
{
    public interface ICodeMstrService  
    {

        #region StandardInterface
        Task<ResultModel> Get(BasicSearchModel searchModel);
        Task<ResultModel> Post(UserMstr codeMstr);
        Task<ResultModel> Put(UserMstr codeMstr);
        new Task<ResultModel> Delete(List<UserMstr> listCodeMstr);
        Task<ResultModel> DownloadTemplate();
        Task<ResultModel> Import(IFormFile file);
        Task<ResultModel> Export(BasicSearchModel searchModel);
        #endregion

        #region CustomInterface
        #endregion

    }
}
