using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using AXIntegracao.Business.Interfaces;
using AXIntegracao.Data.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Linq;
using DapperExtensions;

namespace AXIntegracao.Business.Repositories
{
    public class HistoricoParecerRepository : GenericRepository<HistoricoParecer, HistoricoParecerMapper>, IHistoricoParecerRepository
    {
        private DapperContext dapperContext;
        private Resposta<HistoricoParecer> response;
        public HistoricoParecerRepository(Resposta<HistoricoParecer> response, DapperContext dapperContext) : base(response, dapperContext)
        {
            this.dapperContext = dapperContext;
            this.response = response;
        }

        public Resposta<HistoricoParecer> GetUserById(Guid userId, string conn)
        {
            try
            {
                dapperContext.connection.ConnectionString = conn;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@idHistoricoParecer", userId);
                var dados = SqlMapper.Query<HistoricoParecer>(dapperContext.connection, "spr_listarParecer", parameters, commandType: CommandType.StoredProcedure).ToList();

                response.SetData(dados, 0);

            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Executar Proc",1, ex);
            }

            return response;
        }

        public Resposta<HistoricoParecer> Insert(HistoricoParecer obj, string conn)
        {

            dapperContext.connection.ConnectionString = conn;

            return this.Insert(obj);
        }

        public Resposta<HistoricoParecer> GetBy(PredicateGroup pgMain, object valor, string conn)
        {
            try
            {
                dapperContext.connection.ConnectionString = conn;
                response.SetData(dapperContext.connection.GetList<HistoricoParecer>(pgMain).ToList(), (int)eRespostaStatus.OK);
            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Consultar", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }

        public Resposta<HistoricoParecer> Delete(HistoricoParecer obj, string conn)
        {
            try
            {
                dapperContext.connection.ConnectionString = conn;
                bool statusQuery = dapperContext.connection.Delete(obj);
                response.SetData(obj, (int)eRespostaStatus.OK);

                if (statusQuery)
                    response.Mensagem = "Dado Removido";
                else
                    response.Mensagem = "Nenhum dado Removido";
            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Remover Dado", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }

    }

    
}
