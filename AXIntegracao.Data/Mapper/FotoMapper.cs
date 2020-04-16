using AXIntegracao.Data.Entities;
using DapperExtensions.Mapper;

namespace AXIntegracao.Data.Mapper
{
    public class FotoMapper : ClassMapper<Foto>
    {
        public FotoMapper()
        {
            Table("Foto");
            Map(x => x.Orc).Column("Orc");
            Map(x => x.Num_Val).Column("Num_Val");
            Map(x => x.Numfot).Column("Numfot");
            Map(x => x.Dt_Foto).Column("Dt_Foto");
            Map(x => x.Versao).Column("Versao");
        }
    }
}
