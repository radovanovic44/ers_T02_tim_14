using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IProizvodnjaVinaServis
    {
        
        bool ZapocniFermentaciju(KategorijaVina kategorijaVina, int brojFlasa, double zapreminaFlase);

       
        Vino ZahtevZaVino(Guid id, int kolicina);
    }
}
