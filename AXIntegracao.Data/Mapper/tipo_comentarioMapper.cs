using AXIntegracao.Data.Entities;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIntegracao.Data.Mapper
{
    public class tipo_comentarioMapper : ClassMapper<tipo_comentario>
    {
        public tipo_comentarioMapper()
        {
            Table("tipo_comentario");

            Map(x => x.tipo).Key(KeyType.Assigned).Column("tipo");
            Map(x => x.descricao).Column("descricao");
        }
    }
}
