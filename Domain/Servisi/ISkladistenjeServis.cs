using System;
using System.Collections.Generic;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        bool PrimiPaletu(Paleta paleta);
        List<Paleta> IsporuciPalete(int brojPaleta);

        List<Paleta> IsporuciPaleteZaVino(Guid vinoId, int brojPaleta);
        int DostupnoFlasa(Guid vinoId);

        bool ImaNaStanju(Guid vinoId);

        bool OduzmiFlaseZaVino(Guid vinoId, int kolicina);
    }
}
