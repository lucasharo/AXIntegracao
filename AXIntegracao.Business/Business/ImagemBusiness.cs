using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;
using System;
using System.Linq;

namespace AXIntegracao.Business.Business
{
    public class ImagemBusiness : IImagemBusiness
    {
        IAxWebConfiguracoesRepository configuracoesRepository;
        IImagemRepository imagemRepository;
        Resposta<Foto> resposta;
        public ImagemBusiness(IAxWebConfiguracoesRepository configuracoesRepository, IImagemRepository imagemRepository, Resposta<Foto> resposta)
        {
            this.configuracoesRepository = configuracoesRepository;
            this.imagemRepository = imagemRepository;
            this.resposta = resposta;
        }

        /*/// <summary>
        /// Retorna os dados da imagem do banco mais seu caminho de criação
        /// </summary>
        /// <param name="orc">Número do orçamento</param>
        /// <param name="numeroFoto">Número da foto</param>
        /// <returns>Objeto Foto</returns>
        public Resposta<Foto> RetornaImagem(int orc, int numeroFoto)
        {
            var foto = imagemRepository.ObterDetalheImagem(orc, numeroFoto);
            if (foto.Status == (int)eRespostaStatus.Erro)
                throw new Exception("Foto não foi localizada");
            else
            {
                var caminho = configuracoesRepository.ObterCaminho();
                if (caminho.Status == (int)eRespostaStatus.Erro)
                    throw new Exception("Caminho da foto não foi encontrado");
                else
                {
                    var caminhoDaFoto = foto.Data.FirstOrDefault();
                    caminhoDaFoto.Caminho = caminho.Data.FirstOrDefault().Valor;
                    imagemRepository.ObterArquivoImagem(caminhoDaFoto);
                }
            }
            return resposta;
        }*/
    }
}
