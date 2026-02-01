using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class PaleteRepozitorijum : IPaleteRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public PaleteRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Paleta DodajPaletu(Paleta paleta)
        {
            _baza.Tabele.Palete.Add(paleta);
            _baza.SacuvajPromene();
            return paleta;
        }

        public Paleta PronadjiPaletuPoSifri(string sifra)
        {
            return _baza.Tabele.Palete.First(p => p.Sifra == sifra);
        }

        public IEnumerable<Paleta> SvePalete()
        {
            return _baza.Tabele.Palete;
        }


    }
}
