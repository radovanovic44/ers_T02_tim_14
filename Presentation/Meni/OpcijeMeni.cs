using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly Korisnik korisnik;
        private readonly IProdajaServis _prodajaServis;
        private readonly FaktureMeni _faktureMeni;
        public OpcijeMeni(Korisnik korisnik, FaktureMeni faktureMeni,IProdajaServis prodajaServis)
        {
            this.korisnik = korisnik;
            _faktureMeni = faktureMeni;
            _prodajaServis = prodajaServis;
        }

        public void PrikaziMeni()
        {
            Console.WriteLine("\n============================================ Meni ===========================================");

            bool kraj = false;
            while (!kraj)
            {
                Console.Clear();
                Console.WriteLine("================================ MENI ================================");
                Console.WriteLine($"Prijavljeni korisnik: {korisnik.ImePrezime} ({korisnik.Uloga})");
                Console.WriteLine();

                if (korisnik.Uloga == TipKorisnika.GLAVNI_ENOLOG)
                {
                    Console.WriteLine("1. Pokreni fermentaciju");
                    Console.WriteLine("2. Pregled svih faktura");
                    _faktureMeni.PregledSvihFaktura();
                    Console.WriteLine("Pritisnite bilo koji taster za dalje");
                }
                else if (korisnik.Uloga == TipKorisnika.KELAR_MAJSTOR)
                {
                    Console.WriteLine("1. Prodaja vina");
                    _faktureMeni.KreirajFakturu();
                    Console.WriteLine("Pritisnite bilo koji taster za dalje");

                }

                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");
                string izbor = Console.ReadLine() ?? "";

                if (izbor == "0")
                    kraj = true;
            }
        }
    }

}
