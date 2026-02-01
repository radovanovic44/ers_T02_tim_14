using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IProizvodnjaVinaServis
    {
        // zapocinje fermentaciju na zahtev (pakovanje -> prodaja)
        bool ZapocniFermentaciju(KategorijaVina kategorijaVina, int brojFlasa, double zapreminaFlase);

        // zahteva proizvedena vina iz internog reda
        Vino ZahtevZaVino(Guid id, int kolicina);
    }
}
