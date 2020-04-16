using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;
using AXIntegracao.Data.Mapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AXIntegracao.Fornecimento.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OrcamentoController
    {

        Resposta<CamposOrcamento> response;
        IOrcamentoRepository business;
        private readonly IConfiguration _config;
        string chaveAPI;

        public OrcamentoController(IOrcamentoRepository business, Resposta<CamposOrcamento> response, IHttpContextAccessor _acessor, IConfiguration config)
        {
            this.response = response;
            this.business = business;
            this._config = config;

            this.chaveAPI = _acessor.HttpContext.Request.Headers["ChaveAPI"].ToString();
        }

        [HttpGet("ObterDadosOrcamento/{numeroOrcamento}")]
        public Resposta<CamposOrcamento> ObterDadosOrcamento(string numeroOrcamento)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@orc", numeroOrcamento);
                return business.StoreProcedure(parameters, "spr_sel_dados_orcamento_wf_fornecimento");
            }
            else
            {
                return this.response.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
            }
        }

        private string ObterConnectionString()
        {
            return _config.GetSection("ConnectionString").GetValue<string>(chaveAPI);
        }

    }
}
