using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace AXIntegracao.Business.Repositories
{
    public class GenericRepository<T, Tmap> : IDisposable, IGenericRepository<T> where T : class where Tmap : ClassMapper<T>
    {
        private Resposta<T> response;
        private DapperContext dapperContext;

        public GenericRepository(Resposta<T> response, DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
            this.response = response;
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(Tmap);
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new[]{ typeof(Tmap).Assembly});
        }

        public virtual Resposta<T> Get(Expression<Func<T, object>> predicate, object valor)
        {
            try
            {
                var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pgMain.Predicates.Add(Predicates.Field<T>(predicate, Operator.Eq, valor));
            
                response.SetData(dapperContext.connection.GetList<T>(pgMain).ToList(), (int)eRespostaStatus.OK);
            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Consultar", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }

        public virtual Resposta<T> GetBy(PredicateGroup pgMain, object valor)
        {
            try
            { 

                response.SetData(dapperContext.connection.GetList<T>(pgMain).ToList(), (int)eRespostaStatus.OK);
            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Consultar", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }

        public virtual Resposta<T> GetAll()
        {
            try
            {
                response.SetData(dapperContext.connection.GetList<T>().ToList(), (int)eRespostaStatus.OK);
            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Consultar", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }

        public virtual Resposta<T> Insert(T obj)
        {
            try
            {
                var propertyPK = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(false)
                    .Any(a => a.GetType() == typeof(KeyAttribute))).FirstOrDefault().Name;
                var id = dapperContext.connection.Insert(obj);

                if (!string.IsNullOrEmpty(propertyPK))
                {
                    obj.GetType().GetProperty(propertyPK).SetValue(obj, id);

                    response.SetData(obj, (int)eRespostaStatus.OK);
                }
                else
                    response.SetData(obj, (int)eRespostaStatus.OK);
            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Cadastrar", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }
        
        public virtual Resposta<T> Update(T obj)
        {
            try
            {
                bool statusQuery = dapperContext.connection.Update(obj);

                response.SetData(obj, (int)eRespostaStatus.OK);

                if (statusQuery) 
                   
                    response.Mensagem = "Dado Atualizado"; 
                else 
                                      response.Mensagem = "Nenhum dado Atualizado";

            }
            catch (Exception ex)
            {
                response.SetMessage("Erro ao Atualizar", (int)eRespostaStatus.Erro, ex);
            }

            return response;
        }

        //public virtual Response<T> Delete(long id)
        //{
        //    try
        //    {
        //        //T obj = context.Set<T>().Find(id);
        //        //context.Set<T>().Remove(obj);
        //        //context.SaveChanges();

        //        //response.SetData(obj);
        //        response.Message = "Dado Removido";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.SetMessage("Erro ao Remover Dado", false, ex);
        //    }

        //    return response;
        //}

        public virtual Resposta<T> Delete(T obj)
        {
            try
            {
                bool statusQuery = dapperContext.connection.Delete(obj);
                response.SetData(obj, (int)eRespostaStatus.OK);

                if(statusQuery)
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

        public Resposta<T> StoreProcedure(DynamicParameters parameters, string Proc)
        {
            try
            {   
                var dados = SqlMapper.Query<T>(dapperContext.connection, Proc, parameters, commandType: CommandType.StoredProcedure).ToList();

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