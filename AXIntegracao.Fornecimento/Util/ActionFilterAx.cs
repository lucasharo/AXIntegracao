using AXIntegracao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace AXIntegracao.Fornecimento.Util
{
    //public class ActionFilterAX : IActionFilter
    //{
    //    private readonly IConfiguration _config;
    //    private DapperContext dapperContext;

    //    public ActionFilterAX(IConfiguration config, DapperContext dapperContext)
    //    {
    //        this._config = config;
    //        this.dapperContext = dapperContext;
    //    }

    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        context.HttpContext.Request.Headers.TryGetValue("ChaveAPI", out var cliente);
    //        if (!string.IsNullOrEmpty(cliente))
    //        {
    //            string conn = _config.GetSection("ConnectionString").GetValue<string>(cliente);
    //            this.dapperContext.connection.ConnectionString = conn;
    //            //context.Result = new OkObjectResult(true);
    //        }
    //        else
    //        {
    //            context.Result = new OkObjectResult("Chave Api Inválida!");
    //        }
    //    }

    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {

    //    }
    //}
}
