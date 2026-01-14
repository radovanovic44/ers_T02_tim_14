using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;
namespace Domain.Servisi
{
    public interface IProizvodnjaVinaServis
    {
        void zapocniFermentaciju(KategorijaVina kategorija, int brojFlasa, double zapreminaFlase);
        List<Vino> ZahtevZaVino(KategorijaVina kategorija, int kolicina);
    }
}
