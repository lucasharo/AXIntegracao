using AXIntegracao.Data.Entities;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIntegracao.Data.Mapper
{
    public class HistoricoParecerMapper : ClassMapper<HistoricoParecer>
    { 
        public HistoricoParecerMapper()
        {
            Table("HistoricoParecer");

            //Map(x => x.idHistoricoParecer).Key(KeyType.Identity).Column("idHistoricoParecer");
            Map(x => x.idHistoricoParecer).Key(KeyType.Guid).Column("idHistoricoParecer");
            Map(x => x.Orc).Column("Orc");
            Map(x => x.Complemento).Column("Complemento");
            Map(x => x.CodigoOrdem).Column("CodigoOrdem");
            Map(x => x.DataHoraInclusao).Column("DataHoraInclusao");
            Map(x => x.Usuario).Column("Usuario");
            Map(x => x.Parecer).Column("Parecer");
            Map(x => x.Tipo).Column("Tipo");
            Map(x => x.flagTransmissao).Column("flagTransmissao");
            Map(x => x.FlagBloqueio).Column("FlagBloqueio");
        }
    }
}
