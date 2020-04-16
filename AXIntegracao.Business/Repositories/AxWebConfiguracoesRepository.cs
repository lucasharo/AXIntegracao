using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using AXIntegracao.Data.Mapper;
using DapperExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AXIntegracao.Business.Repositories
{
    public class AxWebConfiguracoesRepository : GenericRepository<AxWeb_Configuracoes, AxWeb_ConfiguracoesMapper>, IAxWebConfiguracoesRepository
    {
        DapperContext dapperContext;
        Resposta<AxWeb_Configuracoes> response;
        IConfiguration config;

        #region Metodos Publicos
        public AxWebConfiguracoesRepository(Resposta<AxWeb_Configuracoes> response, DapperContext dapperContext, IConfiguration config) : base(response, dapperContext)
        {
            this.response = response;
            this.dapperContext = dapperContext;
            this.config = config;
        }

        public Resposta<AxWeb_Configuracoes> ObterCaminho(string connectionString)
        {
            try
            {
                //Altera o contexto de acordo com a requisição.
                dapperContext.connection.ConnectionString = connectionString;
                Expression<Func<AxWeb_Configuracoes, object>> predicate = c => c.Chave;
                var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pgMain.Predicates.Add(Predicates.Field<AxWeb_Configuracoes>(predicate, Operator.Eq, "caminhoimagens"));
                var voCaminho = this.dapperContext.connection.GetList<AxWeb_Configuracoes>(pgMain);
                return this.response.SetData(voCaminho, (int)eRespostaStatus.OK);
            }
            catch (Exception ex)
            {
                this.response.SetMessage("Erro ao Obter Detalhe Imagem", (int)eRespostaStatus.Erro, ex);
                return this.response;
            }
        }
        #endregion
    }
}