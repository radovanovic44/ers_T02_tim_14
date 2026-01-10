using System;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
        // Pakuje zadatu kolicinu pakovanja vina u palete za dati podrum i vino.
        // Vraca par: (stvarno upakovano, paleta u koju je upakovano).
        (bool, Paleta) PakujeVinoUPaletu(Guid vinskiPodrumId, Guid vinoId, int zeljenaKolicinaPakovanja);

        // Vraca prvu dostupnu paletu za dati podrum i vino.
        Paleta? PrvaDostupnaPaleta(Guid vinskiPodrumId, Guid vinoId);
    }
}