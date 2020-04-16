using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AXIntegracao.Data.Entities
{
    public class tipo_comentario
    {
        [Key]
        [StringLength(1)]
        public string tipo { get; set; }

        [StringLength(10)]
        public string descricao { get; set; }
    }
}
