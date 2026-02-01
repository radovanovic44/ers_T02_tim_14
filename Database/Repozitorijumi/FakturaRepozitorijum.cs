using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class FakturaRepozitorijum : IFakturaRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public FakturaRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public bool Dodaj(Faktura faktura)
        {
            try
            {
                _baza.Tabele.Fakture.Add(faktura);
                return _baza.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Faktura> VratiSve()
        {
            return _baza.Tabele.Fakture;
        }
    }
}
