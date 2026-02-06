using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IAutentifikacijaServis
    {
        (bool, Korisnik) Prijava(string korisnickoIme, string lozinka);

        (bool, Korisnik) Registracija(string korisnickoIme, string lozinka, string imePrezime, TipKorisnika uloga);
    }
}
