using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System.Threading;

namespace Services.AutenftikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {
        private readonly IKorisniciRepozitorijum _korisniciRepozitorijum;
        private readonly ILoggerServis _loggerServis;

        public AutentifikacioniServis(IKorisniciRepozitorijum repozitorijum, ILoggerServis logger)
        {
            _korisniciRepozitorijum = repozitorijum;
            _loggerServis = logger;
        }

        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {
            var pronadjen = _korisniciRepozitorijum.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme);

            if (!string.IsNullOrWhiteSpace(pronadjen.KorisnickoIme) && pronadjen.Lozinka == lozinka)
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Korisnik '{korisnickoIme}' je uspešno prijavljen.");
                Console.WriteLine("Korisnik uspešno prijavljen.");
                Thread.Sleep(1200);
                return (true, pronadjen);
            }

            _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Neuspešna prijava za korisnika '{korisnickoIme}'.");
            return (false, new Korisnik());
        }

        public (bool, Korisnik) Registracija(string korisnickoIme, string lozinka, string imePrezime, TipKorisnika uloga)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka) || string.IsNullOrWhiteSpace(imePrezime))
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, "Pokusaj registracije sa praznim poljima.");
                    return (false, new Korisnik());
                }

                var postoji = _korisniciRepozitorijum.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme.Trim());
                if (!string.IsNullOrWhiteSpace(postoji.KorisnickoIme))
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Registracija odbijena: korisnicko ime '{korisnickoIme}' vec postoji.");
                    return (false, new Korisnik());
                }

                var novi = new Korisnik(korisnickoIme.Trim(), lozinka.Trim(), imePrezime.Trim(), uloga);
                var dodat = _korisniciRepozitorijum.DodajKorisnika(novi);

                if (string.IsNullOrWhiteSpace(dodat.KorisnickoIme))
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Registracija neuspesna za '{korisnickoIme}'.");
                    return (false, new Korisnik());
                }

                _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Novi korisnik '{korisnickoIme}' je registrovan ({uloga}).");
                Console.WriteLine("Registracija uspešna. Možete sada da se prijavite.");
                Thread.Sleep(1200);
                return (true, dodat);
            }
            catch (Exception ex)
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska registracije: {ex.Message}");
                return (false, new Korisnik());
            }
        }
    }
}
