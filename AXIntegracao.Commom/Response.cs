using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AXIntegracao.Commom
{
    public class Resposta<T> where T : class
    {
        ILogger log;
        public string Mensagem { get; set; }
        public Exception Ex { get; set; }
        public int Status { get; set; }

        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<T> Data { get; set; }

        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public IEnumerable<T> DataList { get; set; }

        //public Response()
        //{
        //    //  this.log = loggerFactory.CreateLogger(typeof(T).GetType().Name);
        //}

        public Resposta(ILoggerFactory loggerFactory)
        {
            this.log = loggerFactory.CreateLogger(typeof(T).GetType().Name);
        }

        public Resposta<T> SetData(T data, int Status = (int)eRespostaStatus.OK)
        {
            this.Data = new List<T>() { data };
            this.Status = Status;
            this.Mensagem = null;

            return this;
        }
        public Resposta<T> SetData(IEnumerable<T> datalist, int Status = (int)eRespostaStatus.OK)
        {
            this.Data = datalist;
            this.Status = Status;
            this.Mensagem = null;

            return this;
        }
        public Resposta<T> SetMessage(string mensagem, int Status = (int)eRespostaStatus.OK, Exception ex = null)
        {
            this.Mensagem = mensagem;
            this.Status = Status;
            this.Data = null;
            this.Ex = ex;

            if (Status == (int)eRespostaStatus.Erro)
            {
                if (ex == null)
                {
                     this.log.LogWarning(mensagem);
                }
                else
                {
                      this.log.LogError(1, ex, mensagem);
                }
            }

            return this;
        }
    }
}
