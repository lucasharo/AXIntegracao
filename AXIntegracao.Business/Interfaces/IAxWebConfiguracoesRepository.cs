using AXIntegracao.Commom;
using AXIntegracao.Data.Entities;

namespace AXIntegracao.Business.Interfaces
{
    public interface IAxWebConfiguracoesRepository : IGenericRepository<AxWeb_Configuracoes>
    {
        Resposta<AxWeb_Configuracoes> ObterCaminho(string connectionString);
    }
}
