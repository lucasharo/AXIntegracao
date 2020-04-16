using AXIntegracao.Data.Entities;
using DapperExtensions.Mapper;

namespace AXIntegracao.Data.Mapper
{
    public class AxWeb_ConfiguracoesMapper : ClassMapper<AxWeb_Configuracoes>
    {
        public AxWeb_ConfiguracoesMapper()
        {
            Table("AxWeb_Configuracoes");
            Map(x => x.Codigo).Column("Codigo");
            Map(x => x.CodigoCliente).Column("CodigoCliente");
            Map(x => x.Chave).Column("Chave");
            Map(x => x.Valor).Column("Valor");
        }
    }
}
