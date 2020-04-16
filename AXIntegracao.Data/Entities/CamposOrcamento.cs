using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AXIntegracao.Data.Entities
{
    public class CamposOrcamento
    {
        public int NumeroVersao { get; set; }
        public string DataReparo { get; set; }
        public string DiasReparo { get; set; }
        public decimal ValorEstimativaMecanica { get; set; }
        public decimal ValorEstimativaFornecimento { get; set; }
        public decimal ValorMaoDeObra { get; set; }
        public decimal ValorFranquia { get; set; }
        public decimal ValorTotalAvaliado { get; set; }
        public decimal ValorTotalAvarias { get; set; }
    }
}
