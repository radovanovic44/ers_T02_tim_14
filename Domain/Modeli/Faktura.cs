using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.PomocneMetode;

namespace Domain.Modeli
{
    public class Faktura
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Datum { get; set; }

        public TipProdaje TipProdaje { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }

        public List<StavkaFakture> Stavke { get; set; } = new();

        public decimal UkupanIznos =>
            Stavke.Sum(s => s.UkupnaCena);
    }

}
