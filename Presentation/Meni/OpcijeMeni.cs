using Domain.Enumeracije;
using Domain.Modeli;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly Korisnik korisnik;
        public OpcijeMeni(Korisnik korisnik)
        {
            this.korisnik = korisnik;
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
                }
                else if (korisnik.Uloga == TipKorisnika.KELAR_MAJSTOR)
                {
                    Console.WriteLine("1. Prodaja vina");
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
