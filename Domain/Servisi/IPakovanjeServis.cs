using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
        // Pakuje vina u paletu (po kategoriji i zapremini flase) i vraca rezultat i kreiranu paletu.
        (bool, Paleta) UpakujVina(Guid vinskiPodrumId, KategorijaVina kategorija, int brojFlasa, double zapreminaFlase);

        // Salje prvu upakovanu paletu u skladiste (oznaci je kao otpremljenu).
        bool PosaljiPaletuUSkladiste(Guid vinskiPodrumId);
    }
}
