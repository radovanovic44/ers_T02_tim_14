using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.AutenftikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {

        IKorisniciRepozitorijum korisniciRepozitorijum;
        ILoggerServis loggerServis;

        public AutentifikacioniServis(IKorisniciRepozitorijum repozitorijum, ILoggerServis logger)
        {
            korisniciRepozitorijum = repozitorijum;
            loggerServis = logger;
        }

        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {

            Korisnik uspesno_pronadjen = korisniciRepozitorijum.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme);

            if (uspesno_pronadjen.KorisnickoIme != string.Empty && uspesno_pronadjen.Lozinka == lozinka)
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Korisnik '{korisnickoIme}' je uspešno prijavljen.");
                return (true, uspesno_pronadjen);
            }
            else
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Neuspešna prijava za korisnika '{korisnickoIme}'.");
                return (false, new Korisnik());
            }
        }
    }
}
