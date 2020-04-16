using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIntegracao.Business.Interfaces
{
    public interface IHistoricoParecerRepository : IGenericRepository<HistoricoParecer>
    {
        Resposta<HistoricoParecer> GetUserById(Guid userId, string conn);

        Resposta<HistoricoParecer> Insert(HistoricoParecer obj, string conn);

        Resposta<HistoricoParecer> GetBy(PredicateGroup pgMain, object valor, string conn);

        Resposta<HistoricoParecer> Delete(HistoricoParecer obj, string conn);


    }
}
