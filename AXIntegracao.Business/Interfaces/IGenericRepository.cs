using AXIntegracao.Commom;
using Dapper;
using DapperExtensions;
using System;
using System.Linq.Expressions;

namespace AXIntegracao.Business.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Resposta<T> Get(Expression<Func<T, object>> predicate, object valor);
        Resposta<T> GetBy(PredicateGroup pgMain, object valor);
        Resposta<T> GetAll();
        Resposta<T> Insert(T Entidade);
        Resposta<T> Update(T Entidade);
        Resposta<T> Delete(T Entidade);
        Resposta<T> StoreProcedure(DynamicParameters parameters, string Proc);
    }
}
