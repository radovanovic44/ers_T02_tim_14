using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Authentifikacija
{
    public class AutentifikacioniMeni
    {
        private readonly IAutentifikacijaServis _autentifikacijaServis;

        public AutentifikacioniMeni(IAutentifikacijaServis autentifikacijaServis)
        {
            _autentifikacijaServis = autentifikacijaServis;
        }

        /// <summary>
        /// Glavni meni za autentifikaciju (login/registracija/izlaz).
        /// Vraca true kad je korisnik uspesno prijavljen.
        /// Vraca false kad korisnik izabere izlaz iz aplikacije.
        /// </summary>
        public bool Authenticate(out Korisnik korisnik)
        {
            korisnik = new Korisnik();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================== AUTENTIFIKACIJA ====================");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Registracija");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");

                var izbor = Console.ReadLine() ?? "";
                switch (izbor.Trim())
                {
                    case "1":
                        if (TryLogin(out korisnik))
                            return true;

                        Console.WriteLine("Pogrešno korisničko ime ili lozinka.");
                        Pause();
                        break;

                    case "2":
                        TryRegister();
                        Pause();
                        break;

                    case "0":
                        return false;

                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        Pause();
                        break;
                }
            }
        }

        public bool TryLogin(out Korisnik korisnik)
        {
            korisnik = new Korisnik();

            Console.Clear();
            Console.WriteLine("==================== LOGIN MENI ====================");
            Console.Write("Korisničko ime: ");
            var korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            var lozinka = Console.ReadLine() ?? "";

            var (uspesnaPrijava, prijavljen) = _autentifikacijaServis.Prijava(korisnickoIme.Trim(), lozinka.Trim());
            korisnik = prijavljen;
            return uspesnaPrijava;
        }

        private bool TryRegister()
        {
            Console.Clear();
            Console.WriteLine("==================== REGISTRACIJA ====================");
            Console.Write("Korisničko ime: ");
            var korisnickoIme = (Console.ReadLine() ?? "").Trim();

            Console.Write("Ime i prezime: ");
            var imePrezime = (Console.ReadLine() ?? "").Trim();

            Console.Write("Lozinka: ");
            var lozinka1 = (Console.ReadLine() ?? "").Trim();

            Console.Write("Potvrda lozinke: ");
            var lozinka2 = (Console.ReadLine() ?? "").Trim();

            if (lozinka1 != lozinka2)
            {
                Console.WriteLine("Lozinke se ne poklapaju.");
                return false;
            }

            Console.WriteLine("Uloga: 1-GLAVNI_ENOLOG, 2-KELAR_MAJSTOR");
            Console.Write("Izbor uloge: ");
            var u = (Console.ReadLine() ?? "").Trim();
            var uloga = u == "1" ? TipKorisnika.GLAVNI_ENOLOG : TipKorisnika.KELAR_MAJSTOR;

            var (ok, _) = _autentifikacijaServis.Registracija(korisnickoIme, lozinka1, imePrezime, uloga);
            if (!ok)
            {
                Console.WriteLine("Registracija nije uspela (korisničko ime možda već postoji ili su podaci neispravni).");
                return false;
            }

            return true;
        }

        private static void Pause()
        {
            Console.WriteLine("\nPritisnite bilo koji taster za nastavak...");
            Console.ReadKey();
        }
    }
}
