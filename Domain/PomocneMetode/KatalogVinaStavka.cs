using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.PomocneMetode
{
    public class KatalogVinaStavka
    {
        public Guid VinoId { get; set; } = Guid.NewGuid();
        public string Naziv { get; set; } = string.Empty;
        public KategorijaVina Kategorija { get; set; }
        public double Zapremina { get; set; } = 0;

        public int BrojFlasa { get; set; }
    }
}
