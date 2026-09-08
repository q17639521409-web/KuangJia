using FrameWebAPI.IService.Master;
using FrameWebAPI.Model.Basic;
using FrameWebAPI.Model.Model;
using FrameWebAPI.Service.Extensions;
using FrameWebAPI.WebAPI.Controllers.Basic;
using FrameWebAPI.WebAPI.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrameWebAPI.WebAPI.Controllers.Master
{
    public class CodeMstrController : BasicController
    {
        private readonly ILogger<CodeMstrController> _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICodeMstrService _codemstrService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codemstrService"></param>
        /// <param name="accessor"></param>
        /// <param name="logger"></param>
        public CodeMstrController(IHttpContextAccessor accessor, ILogger<CodeMstrController> logger, ICodeMstrService codemstrService)
        {
            _accessor = accessor;
            _logger = logger;
            _codemstrService = codemstrService;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchModel">查询参数</param>
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] BasicSearchModel searchModel)
        {
            return Ok(await _codemstrService.Get(searchModel));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="codemstr"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserMstr codemstr)
        {
            return Ok(await _codemstrService.Post(codemstr));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="codemstr"></param>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserMstr codemstr)
        {
            return Ok(await _codemstrService.Put(codemstr));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="listCodeMstr"></param>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<UserMstr> listCodeMstr)
        {
            return Ok(await _codemstrService.Delete(listCodeMstr));
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DownloadTemplate()
        {
            ResultModel rm = await _codemstrService.DownloadTemplate();
            if (rm.ResultCode != "100000")
            {
                return Ok(rm);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(rm.Data.GetCString());
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "codemstrLoad.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="file"></param>
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            return Ok(await _codemstrService.Import(file));
        }

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="searchModel">查询参数</param>
        [HttpPost]
        public async Task<IActionResult> Export([FromBody] BasicSearchModel searchModel)
        {
            ResultModel rm = await _codemstrService.Export(searchModel);
            if (rm.ResultCode != "100000")
            {
                return Ok(rm);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(rm.Data.GetCString());
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "codemstrExport.xlsx");
        }

    }
}
