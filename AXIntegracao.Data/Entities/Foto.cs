using System;

namespace AXIntegracao.Data.Entities
{
    public partial class Foto
    {
        public int Orc { get; set; }
        public int Num_Val { get; set; }
        public short Numfot { get; set; }
        public DateTime? Dt_Foto { get; set; }
        public int? Versao { get; set; }
        public string foto { get; set; }
        public string thumb { get; set; }
        public string Caminho { get; set; }
    }

    public partial class FotoLista
    {
        public int Codigo { get; set; }
        public int? Versao { get; set; }
        public short NumeroFoto { get; set; }
        public DateTime? DataInclusao { get; set; }
    }

    public partial class FotoDetalhe
    {
        public string Foto { get; set; }
        public string Thumb { get; set; }
    }
}
