using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode
{
    public class StavkaFakture
    {
        public Guid VinoId { get; set; }
        public string NazivVina { get; set; } = string.Empty;

        public int Kolicina { get; set; }
        public decimal CenaPoFlasi { get; set; }

        public decimal UkupnaCena => Kolicina * CenaPoFlasi;
    }
}
