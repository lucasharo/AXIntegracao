using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using AXIntegracao.Data.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AXIntegracao.Pareceres.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    public class TipoComentarioController : GenericController<tipo_comentario, tipo_comentarioMapper>
    {
        DapperContext dapperContext;
        Itipo_comentarioRepository business;
        Resposta<tipo_comentario> response;
        private readonly IConfiguration _config;
        string chaveApi;

        public TipoComentarioController(Itipo_comentarioRepository business, Resposta<tipo_comentario> response, DapperContext _dapperContext, IHttpContextAccessor _acessor, IConfiguration config) : base(business, response)
        {
            this.business = business;
            this.response = response;
            this.dapperContext = _dapperContext;
            this._config = config;
            _acessor.HttpContext.Request.Headers.TryGetValue("ChaveAPI", out var cliente);
            this.chaveApi = cliente;
        }

        [HttpGet("{id}")]
        public ActionResult<Resposta<tipo_comentario>> Obter(string id)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                dapperContext.connection.ConnectionString = conn;
                Expression<Func<tipo_comentario, object>> predicate = c => c.tipo;
                response = business.Get(predicate, id);
            }
            else
            {
                return this.response.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
            }

            return response;
        }

        [HttpDelete("{id}")]
        public ActionResult<Resposta<tipo_comentario>> Deletar(string valor)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                Expression<Func<tipo_comentario, object>> predicate = c => c.tipo;

                var retorno = business.Get(predicate, valor);
                if (retorno.Status.Equals((int)eRespostaStatus.OK))
                {
                    response = business.Delete(retorno.Data.FirstOrDefault());
                }
                else
                {
                    response.SetMessage("Erro ao Deletar", (int)eRespostaStatus.Erro, retorno.Ex);
                }
            }
            else
            {
                return this.response.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
            }

            return response;
        }


        private string ObterConnectionString()
        {
            return _config.GetSection("ConnectionString").GetValue<string>(chaveApi);
        }

    }
}
