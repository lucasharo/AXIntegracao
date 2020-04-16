using AXIntegracao.Business.Interfaces;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using AXIntegracao.Data.Mapper;
using DapperExtensions;
using Ionic.Zip;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace AXIntegracao.Business.Repositories
{
    public class ImagemRepository : GenericRepository<Foto, FotoMapper>, IImagemRepository
    {
        DapperContext dapperContext;
        Resposta<Foto> response;
        Resposta<FotoLista> responseLista;
        Resposta<FotoDetalhe> responseDetalhe;
        IConfiguration config;
        IAxWebConfiguracoesRepository configuracoes;


        #region Metodos Publicos
        public ImagemRepository(Resposta<Foto> response, 
            DapperContext dapperContext,
            Resposta<FotoLista> responseLista,
            Resposta<FotoDetalhe> responseDetalhe,
            IConfiguration config,
            IAxWebConfiguracoesRepository _configuracoes) : base(response, dapperContext)
        {
            this.response = response;
            this.dapperContext = dapperContext;
            this.config = config;
            this.responseLista = responseLista;
            this.responseDetalhe = responseDetalhe;
            this.configuracoes = _configuracoes;
        }
        public Resposta<FotoLista> ListarImagens(int orc, string connectionString)
        {
            dapperContext.connection.ConnectionString = connectionString;

            Expression<Func<Foto, object>> predicate = c => c.Orc;
            this.response = this.Get(predicate, orc);
            var lista = new List<FotoLista>();

            foreach (var foto in response.Data) {
                lista.Add(new FotoLista {
                    Codigo = foto.Orc,
                    DataInclusao = foto.Dt_Foto,
                    NumeroFoto = foto.Numfot,
                    Versao = foto.Versao
                });
            }

            if (lista.Count > 0)
            {
                return responseLista.SetData(lista, (int)eRespostaStatus.OK);
            }
            else
            {
                return responseLista.SetMessage("Esse orçamento não possui imagens", (int)eRespostaStatus.OK);
            }
        }

        public Resposta<FotoDetalhe> ObterDetalheImagem(int orc, int numeroFoto, string connectionString)
        {
            try
            {
                //Altera o contexto de acordo com a requisição.
                dapperContext.connection.ConnectionString = connectionString;
                Expression<Func<Foto, object>> predicateOrc = c => c.Orc;
                Expression<Func<Foto, object>> predicateFoto = c => c.Numfot;

                var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pgMain.Predicates.Add(Predicates.Field<Foto>(predicateOrc, Operator.Eq, orc));
                pgMain.Predicates.Add(Predicates.Field<Foto>(predicateFoto, Operator.Eq, numeroFoto));

                var voFoto = this.dapperContext.connection.GetList<Foto>(pgMain).FirstOrDefault();

                var arquivoResponse = ObterArquivoImagem(voFoto, connectionString);

                if (arquivoResponse.Status == 0)
                {
                    var arquivo = arquivoResponse.Data.FirstOrDefault();

                    var fotoDetalhe = new FotoDetalhe
                    {
                        Foto = arquivo.foto,
                        Thumb = arquivo.thumb
                    };

                    return this.responseDetalhe.SetData(fotoDetalhe, (int)eRespostaStatus.OK);
                }
                else
                {
                    return this.responseDetalhe.SetMessage("Imagem não encontrada", (int)eRespostaStatus.OK);
                }
            }
            catch (Exception ex)
            {
                return this.responseDetalhe.SetMessage("Erro ao Obter Detalhe Imagem", (int)eRespostaStatus.Erro, ex);
            }
        }

        #endregion
        #region Metodos Private
        /// <summary>
        /// Devolve o objeto valor com a string de 64bits da imagem
        /// </summary>
        /// <param name="voFoto"></param>
        /// <returns>Objeto Foto</returns>
        public Resposta<Foto> ObterArquivoImagem(Foto voFoto, string connectionString)
        {
            Resposta<AxWeb_Configuracoes> respostaPathImagem = configuracoes.ObterCaminho(connectionString);
            AxWeb_Configuracoes pathImagens = respostaPathImagem.Data.FirstOrDefault<AxWeb_Configuracoes>();
            string pathImagem = pathImagens.Valor;
            string pathAux = config.GetValue<string>("pathAux");

            //Verifica se as informações principais vieram
            if (voFoto == null) return null;
            if (voFoto.Orc < 0) return null;
            if (voFoto.Numfot == 0) return null;

            var tempPath = Path.Combine(pathAux, Guid.NewGuid().ToString());
            try
            {
                //Define os nomes dos arquivos fisicos
                var nomeArquivo = $"{voFoto.Orc}.fot";
                var file = Path.Combine(pathImagem, nomeArquivo);
                var fileTemp = Path.Combine(tempPath, nomeArquivo);

                //Verifica se existe o arquivo fisicamente
                if (!File.Exists(file))
                    throw new FileNotFoundException($"Arquivo {voFoto.Orc}.fot não encontrado.");

                //Cria a pasta, caso a mesma não exista
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);

                //Copia o arquivo para um diretorio temporário
                File.Copy(file, fileTemp);

                //Faz a leitura dentro do arquivo zipado
                using (var zip = ZipFile.Read(fileTemp))
                {
                    var foto = $"{voFoto.Orc.ToString().Trim()}{voFoto.Numfot.ToString().Trim().PadLeft(2,'0')}.M00";
                    var tumb = $"{voFoto.Orc.ToString().Trim()}{voFoto.Numfot.ToString().Trim().PadLeft(2,'0')}.T00";
                    var fotoResultado = zip.Entries.Where(e => e.FileName.ToUpper().Equals(foto) || e.FileName.ToUpper().Equals(tumb)).ToList();
                    if (fotoResultado.Count < 2)
                        throw new FileNotFoundException($"Arquivos {foto} e {tumb} não foram encontrados.");

                    using (var ms = new MemoryStream())
                    {
                        for (int i = 0; i < fotoResultado.Count(); i++)
                        {
                            fotoResultado[i].Password = "w3R@hTTo$23oIUcXq&84!A";
                            fotoResultado[i].Extract(ms);

                            var buffer = new byte[ms.Length];
                            ms.Position = 0;
                            ms.Read(buffer, 0, Convert.ToInt32(ms.Length));

                            if (fotoResultado[i].FileName.ToUpper().IndexOf('M') > 0)
                                voFoto.foto = Convert.ToBase64String(buffer);
                            else if (fotoResultado[i].FileName.ToUpper().IndexOf('T') > 0)
                                voFoto.thumb = Convert.ToBase64String(buffer);
                        }

                        this.response.SetData(voFoto);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                this.response.SetMessage("Imagem não encontrada", (int)eRespostaStatus.Erro, ex);
            }

            return this.response;
        }
        #endregion
    }
}
