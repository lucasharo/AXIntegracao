using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AXIntegracao.Data.Entities
{
    public class HistoricoParecer
    {
            [Key]
            public Guid idHistoricoParecer { get; set; }
            public int Orc { get; set; }
            public int Complemento { get; set; }
            public int CodigoOrdem { get; set; }
            public DateTime DataHoraInclusao { get; set; }

            [StringLength(30)]
            public string Usuario { get; set; }
            public string Parecer { get; set; }

            [StringLength(10)]
            public string Tipo { get; set; }
            public int flagTransmissao { get; set; }
            public int FlagBloqueio { get; set; }
        }
    }

