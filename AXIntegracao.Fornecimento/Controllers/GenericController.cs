using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using DapperExtensions.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace AXIntegracao.Fornecimento.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GenericController<T, Tmap> : ControllerBase where T : class where Tmap : ClassMapper<T>
    {
        private IGenericRepository<T> business;
        private Resposta<T> response;

        public GenericController(IGenericRepository<T> business, Resposta<T> response)
        {
            this.business = business;
            this.response = response;
        }

        [HttpGet]
        //#if !DEBUG
        //        [Authorize(Roles = "ListarTodos")]
        //#endif
        //[ApiExplorerSettings(IgnoreApi = false)]
        public virtual ActionResult<Resposta<T>> ListarTodos()
        {
            var dados = business.GetAll();
            return Ok(dados);
        }

        //[ApiExplorerSettings(IgnoreApi = false)]
        //[HttpGet("{id}")]
        //public virtual ActionResult<Response<T>> Obter(string id)
        //{
        //    Expression<Func<T, object>> predicate = c.
        //    var dado = business.Get(id);
        //    return Ok(response);
        //}

        [HttpPost]
        public virtual ActionResult<Resposta<T>> Cadastrar([FromBody] T obj)
        {
            var dado = business.Insert(obj);
            return Ok(dado);
        }

        [HttpPut("{id}")]
        public virtual ActionResult<Resposta<T>> Atualizar(long id, [FromBody] T obj)
        {
            var dado = business.Update(obj);
            return Ok(dado);
        }

        //[HttpDelete("{id}")]
        //public virtual ActionResult<Response<T>> Deletar(string id)
        //{
        //    //var dado = business.Delete(id);

        //    //response.Status = dado.Status ? 0 : 1;
        //    //response.Mensagem = dado.Message;
        //    //response.SetData(_mapper.Map<T, K>(dado.Data));

        //    return Ok(response);
        //}
    }
}
