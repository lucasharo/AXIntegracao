using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;

namespace AXIntegracao.Business.Interfaces
{
    public interface IImagemRepository : IGenericRepository<Foto>
    {
        Resposta<FotoLista> ListarImagens(int orc, string connectionString);
        Resposta<FotoDetalhe> ObterDetalheImagem(int orc, int numeroFoto, string connectionString);
    }
}
