using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using Dapper;
using System;
using System.Data;
using System.Linq;

namespace AXIntegracao.Business.Repositories
{
    public class OrcamentoRepository: IOrcamentoRepository, IDisposable
    {
        private Resposta<CamposOrcamento> response;
        private DapperContext dapperContext;

        public OrcamentoRepository(Resposta<CamposOrcamento> response, DapperContext dapperContext)
        {
            this.response = response;
            this.dapperContext = dapperContext;
        }

        public Resposta<CamposOrcamento> StoreProcedure(DynamicParameters parameters, string Proc)
        {
            try
            {
                var dados = SqlMapper.Query<CamposOrcamento>(dapperContext.connection, Proc, parameters, commandType: CommandType.StoredProcedure).ToList();

                response.SetData(dados, 0);

            }
            catch (Exception ex)
            {
                response.SetMessage($"Erro ao Executar Procedure: {Proc}", 1, ex);
            }

            return response;
        }

        public void Dispose()
        {
            // context.Dispose();
        }
    }
}
