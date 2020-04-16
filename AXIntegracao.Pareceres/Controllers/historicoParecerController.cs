using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using AXIntegracao.Data.Mapper;
using DapperExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AXIntegracao.Pareceres.Controllers
{

    [ApiVersion("2.0")]
    [ApiController]
    public class HistoricoParecerController : GenericController<HistoricoParecer, HistoricoParecerMapper>
    {
        DapperContext dapperContext;
        IHistoricoParecerRepository business;
        Resposta<HistoricoParecer> response;
        private readonly IConfiguration _config;
        string chaveAPI;

        public HistoricoParecerController(IHistoricoParecerRepository business, Resposta<HistoricoParecer> response, DapperContext _dapperContext, IHttpContextAccessor _acessor, IConfiguration config) : base(business, response)
        {
            this.business = business;
            this.dapperContext = _dapperContext;
            this.response = response;
            this._config = config;

            this.chaveAPI = _acessor.HttpContext.Request.Headers["ChaveAPI"].ToString();
        }

        [HttpGet("{id}")]
        public ActionResult<Resposta<HistoricoParecer>> ObterHistorico(string id)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                dapperContext.connection.ConnectionString = conn;
                Expression<Func<HistoricoParecer, object>> predicate;

                var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                predicate = c => c.idHistoricoParecer;
                pgMain.Predicates.Add(Predicates.Field<HistoricoParecer>(predicate, Operator.Eq, id));
                predicate = c => c.Tipo;
                pgMain.Predicates.Add(Predicates.Field<HistoricoParecer>(predicate, Operator.Eq, "FornecPeca"));

                response = business.GetBy(pgMain, id, conn);
            }
            else
            {
                return this.response.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
            }

            return response;
        }

        [HttpGet("ObterHistoricoByOrcamento/{id}")]

        public ActionResult<Resposta<HistoricoParecer>> ObterHistoricoByOrcamento(int id)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                //dapperContext.connection.ConnectionString = conn;
                Expression<Func<HistoricoParecer, object>> predicate;

                var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                predicate = c => c.Orc;
                pgMain.Predicates.Add(Predicates.Field<HistoricoParecer>(predicate, Operator.Eq, id));
                predicate = c => c.Tipo;
                pgMain.Predicates.Add(Predicates.Field<HistoricoParecer>(predicate, Operator.Eq, "FornecPeca"));

                response = business.GetBy(pgMain, id, conn);
            }
            else
            {
                return this.response.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
            }

            return response;
        }

        //public ActionResult<Resposta<HistoricoParecer>> ObterHistoricoByOrcamento(int id)
        //{
        //    Expression<Func<HistoricoParecer, object>> predicate = c => c.Orc;

        //    response = business.Get(predicate, id);

        //    return response;
        //}

        [HttpDelete("{id}")]
        public ActionResult<Resposta<HistoricoParecer>> Deletar(string id)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                //dapperContext.connection.ConnectionString = conn;
                Expression<Func<HistoricoParecer, object>> predicate = c => c.idHistoricoParecer;

                var retorno = business.Get(predicate, id);
                if (retorno.Status.Equals((int)eRespostaStatus.OK))
                {
                    response = business.Delete(retorno.Data.FirstOrDefault(), conn);
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

        [HttpPost]
        public override ActionResult<Resposta<HistoricoParecer>> Cadastrar([FromBody] HistoricoParecer obj)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                //dapperContext.connection.ConnectionString = conn;
                obj.Tipo = "FornecPeca";
                return business.Insert(obj, conn);
            }
            else
            {
                return this.response.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
            }
        }

        public override ActionResult<Resposta<HistoricoParecer>> Atualizar(long id, [FromBody] HistoricoParecer obj)
        {
            var conn = ObterConnectionString();
            if (!string.IsNullOrEmpty(conn))
            {
                response.SetMessage("Você não tem permissão para atualizar o registro");
                return response;
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
