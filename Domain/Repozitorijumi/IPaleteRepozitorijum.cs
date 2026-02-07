using System.Collections.Generic;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IPaleteRepozitorijum
    {
        Paleta DodajPaletu(Paleta paleta);
        Paleta PronadjiPaletuPoSifri(string sifra);
        IEnumerable<Paleta> SvePalete();
        bool SacuvajIzmene();
    }
}