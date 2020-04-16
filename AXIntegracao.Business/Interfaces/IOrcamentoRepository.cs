using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIntegracao.Business.Interfaces
{
    public interface IOrcamentoRepository// : IGenericRepository<CamposOrcamento>
    {
        Resposta<CamposOrcamento> StoreProcedure(DynamicParameters parameters, string Proc);
    }
}
