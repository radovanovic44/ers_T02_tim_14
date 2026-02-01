using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class KorisniciRepozitorijum : IKorisniciRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public KorisniciRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Korisnik DodajKorisnika(Korisnik korisnik)
        {
            try
            {
                var postoji = PronadjiKorisnikaPoKorisnickomImenu(korisnik.KorisnickoIme);
                if (!string.IsNullOrWhiteSpace(postoji.KorisnickoIme))
                    return new Korisnik(); // vec postoji

                korisnik.Id = Guid.NewGuid();
                _baza.Tabele.Korisnici.Add(korisnik);
                _baza.SacuvajPromene();
                return korisnik;
            }
            catch
            {
                return new Korisnik();
            }
        }

        public Korisnik PronadjiKorisnikaPoKorisnickomImenu(string korisnickoIme)
        {
            try
            {
                return _baza.Tabele.Korisnici.FirstOrDefault(k => k.KorisnickoIme == korisnickoIme) ?? new Korisnik();
            }
            catch
            {
                return new Korisnik();
            }
        }

        public IEnumerable<Korisnik> SviKorisnici()
        {
            try
            {
                return _baza.Tabele.Korisnici;
            }
            catch
            {
                return [];
            }
        }
    }
}
