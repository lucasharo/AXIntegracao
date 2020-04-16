using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Data.Entities;
using AXIntegracao.Business.Interfaces;
using AXIntegracao.Data.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIntegracao.Business.Repositories
{
    public class tipo_comentarioRepository : GenericRepository<tipo_comentario, tipo_comentarioMapper>, Itipo_comentarioRepository
    {
        public tipo_comentarioRepository(Resposta<tipo_comentario> response, DapperContext dapperContext) : base(response, dapperContext)
        {
        }
    }
}
