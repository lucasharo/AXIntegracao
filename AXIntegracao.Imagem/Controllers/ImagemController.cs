using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;
using AXIntegracao.Data.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq.Expressions;

namespace AXIntegracao.Imagem.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ImagemController
    {
        IImagemRepository imagemRepository;
        IImagemBusiness imagemBusiness;
        Resposta<FotoLista> responseLista;
        Resposta<FotoDetalhe> responseDetalhe;
        private readonly IConfiguration _config;
        string chaveAPI;

        public ImagemController(IImagemRepository imagemRepository, Resposta<FotoLista> responseLista, Resposta<FotoDetalhe> responseDetalhe, IImagemBusiness imagemBusiness, IHttpContextAccessor _acessor, IConfiguration config)
        {
            this.imagemRepository = imagemRepository;
            this.imagemBusiness = imagemBusiness;
            this.responseLista = responseLista;
            this.responseDetalhe = responseDetalhe;
            this._config = config;

            this.chaveAPI = _acessor.HttpContext.Request.Headers["ChaveAPI"].ToString();
        }

        [HttpGet("ListarImagens")]
        public ActionResult<Resposta<FotoLista>> ListarImagens([FromQuery]int orc)
        {
            try
            {
                var conn = ObterConnectionString();
                if (!string.IsNullOrEmpty(conn))
                {
                    this.responseLista = imagemRepository.ListarImagens(orc, conn);
                }
                else
                {
                    return this.responseLista.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
                }

                return this.responseLista;
            }
            catch (Exception ex)
            {
                return this.responseLista.SetMessage("Metodo ImagemController", (int)eRespostaStatus.Erro, ex);
            }
        }

        [HttpGet("ObterDetalheImagem")]
        public ActionResult<Resposta<FotoDetalhe>> ObterDetalheImagem([FromQuery]int orc, [FromQuery]int numeroFoto)
        {
            try
            {
                var conn = ObterConnectionString();
                if (!string.IsNullOrEmpty(conn))
                {
                    this.imagemRepository.ObterDetalheImagem(orc, numeroFoto, conn);
                }
                else
                {
                    return this.responseDetalhe.SetMessage("Chave Api Inválida!", (int)eRespostaStatus.Erro, null);
                }
                return this.responseDetalhe;
            }
            catch (Exception ex)
            {
                return this.responseDetalhe.SetMessage("Metodo ObterDetalheImagem", (int)eRespostaStatus.Erro, ex);
            }
        }

        private string ObterConnectionString()
        {
            return _config.GetSection("ConnectionString").GetValue<string>(chaveAPI);
        }
    }
}