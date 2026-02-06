using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;
using System.Linq;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly Korisnik _korisnik;
        private readonly IProizvodnjaVinaServis _proizvodnja;
        private readonly IPakovanjeServis _pakovanje;
        private readonly KatalogVinaMeni _katalogMeni;
        private readonly FaktureMeni _faktureMeni;
        private readonly Guid _vinskiPodrumId;

        public OpcijeMeni(
            Korisnik korisnik,
            IProizvodnjaVinaServis proizvodnja,
            IPakovanjeServis pakovanje,
            KatalogVinaMeni katalogMeni,
            FaktureMeni faktureMeni,
            Guid vinskiPodrumId)
        {
            _korisnik = korisnik;
            _proizvodnja = proizvodnja;
            _pakovanje = pakovanje;
            _katalogMeni = katalogMeni;
            _faktureMeni = faktureMeni;
            _vinskiPodrumId = vinskiPodrumId;
        }

        public bool PrikaziMeni()
        {
            while (true)
            {
                Console.Clear();

                if (_korisnik.Uloga == TipKorisnika.GLAVNI_ENOLOG)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.Clear();
                    Console.WriteLine("================================ MENI ================================");
                    Console.WriteLine($"Prijavljeni korisnik: {_korisnik.ImePrezime} ({_korisnik.Uloga})");
                    Console.WriteLine();
                    Console.WriteLine("1. Pokreni fermentaciju");
                    Console.WriteLine("2. Upakuj vina u paletu (izbor serije)");
                    Console.WriteLine("3. Posalji paletu u skladiste (izbor palete)");
                    Console.WriteLine("4. Pregled svih faktura");
                    Console.WriteLine("5. Katalog vina");
                }
                else // KELAR_MAJSTOR
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.Clear();
                    Console.WriteLine("================================ MENI ================================");
                    Console.WriteLine($"Prijavljeni korisnik: {_korisnik.ImePrezime} ({_korisnik.Uloga})");
                    Console.WriteLine();
                    Console.WriteLine("1. Katalog vina");
                    Console.WriteLine("2. Prodaja (kreiranje fakture)");
                    Console.WriteLine("3. Pregled svih faktura");
                }

                Console.WriteLine("0. Logout");

                Console.Write("Izbor: ");
                var izbor = Console.ReadLine() ?? "";

                if (izbor == "0")
                {
                    Console.WriteLine("Korisnik uspesno izlogovan, unesite enter za ponovno logovanje");
                    Console.ReadLine();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return false;
                }

                if (_korisnik.Uloga == TipKorisnika.GLAVNI_ENOLOG)
                {
                    switch (izbor)
                    {
                        case "1":
                            PokreniFermentaciju();
                            break;
                        case "2":
                            Upakuj();
                            break;
                        case "3":
                            PosaljiPaletu();
                            break;
                        case "4":
                            _faktureMeni.PregledSvihFaktura();
                            Pause();
                            break;
                        case "5":
                            _katalogMeni.Prikazi();
                            Pause();
                            break;
                    }
                }
                else
                {
                    switch (izbor)
                    {
                        case "1":
                            _katalogMeni.Prikazi();
                            Pause();
                            break;
                        case "2":
                            _faktureMeni.KreirajFakturu();
                            Pause();
                            break;
                        case "3":
                            _faktureMeni.PregledSvihFaktura();
                            Pause();
                            break;
                    }
                }
            }
        }

        private bool PokreniFermentaciju()
        {
            Console.WriteLine("Kategorija: 1-Stolno, 2-Kvalitetno, 3-Premium");
            var kat = Console.ReadLine();
            var kategorija = kat switch
            {
                "2" => KategorijaVina.Kvalitetno,
                "3" => KategorijaVina.Premium,
                _ => KategorijaVina.Stolno
            };

            Console.Write("Broj flasa: ");
            if (!int.TryParse(Console.ReadLine(), out var broj) || broj <= 0)
            {
                Console.WriteLine("Neispravan broj flasa.");
                Pause();
                return false;
            }

            Console.Write("Zapremina flase (0.75 ili 1.5): ");
            if (!double.TryParse(Console.ReadLine(), out var zap) || (zap != 0.75 && zap != 1.5))
            {
                Console.WriteLine("Neispravna zapremina.");
                Pause();
                return false;
            }

            var ok = _proizvodnja.ZapocniFermentaciju(kategorija, broj, zap);
            Console.WriteLine(ok ? "Fermentacija pokrenuta." : "Fermentacija nije uspela.");
            Pause();
            return ok;
        }

        private bool Upakuj()
        {
            Console.WriteLine("Kategorija: 1-Stolno, 2-Kvalitetno, 3-Premium");
            var kat = Console.ReadLine();
            var kategorija = kat switch
            {
                "2" => KategorijaVina.Kvalitetno,
                "3" => KategorijaVina.Premium,
                _ => KategorijaVina.Stolno
            };

            var vina = _pakovanje.VratiVinaZaPakovanje(kategorija).ToList();
            if (vina.Count == 0)
            {
                Console.WriteLine("Nema dostupnih serija vina za pakovanje u ovoj kategoriji.");
                Pause();
                return false;
            }

            Console.WriteLine("\nDostupne serije vina:");
            for (int i = 0; i < vina.Count; i++)
            {
                var v = vina[i];
                Console.WriteLine($"{i + 1}. {v.Naziv} | Serija: {v.SifraSerije} | {v.Zapremina}L | Kolicina: {v.KolicinaFlasa}");
            }

            Console.Write("Izaberite seriju (broj): ");
            if (!int.TryParse(Console.ReadLine(), out var izbor) || izbor < 1 || izbor > vina.Count)
            {
                Console.WriteLine("Neispravan izbor.");
                Pause();
                return false;
            }

            var izabranoVino = vina[izbor - 1];

            Console.Write("Broj flasa za pakovanje (max 24): ");
            if (!int.TryParse(Console.ReadLine(), out var broj) || broj <= 0)
            {
                Console.WriteLine("Neispravan broj flasa.");
                Pause();
                return false;
            }

            Console.Write("Zapremina flase (0.75 ili 1.5): ");
            if (!double.TryParse(Console.ReadLine(), out var zap) || (zap != 0.75 && zap != 1.5))
            {
                Console.WriteLine("Neispravna zapremina.");
                Pause();
                return false;
            }

            var (ok, paleta) = _pakovanje.UpakujVina(_vinskiPodrumId, izabranoVino.Id, broj, zap);
            Console.WriteLine(ok
                ? $"Upakovana paleta: {paleta.Sifra} | Serija: {paleta.Vino.SifraSerije} | {paleta.Vino.KolicinaFlasa} flasa"
                : "Pakovanje nije uspelo.");
            Pause();
            return ok;
        }

        private bool PosaljiPaletu()
        {
            var palete = _pakovanje.VratiUpakovanePalete(_vinskiPodrumId).ToList();
            if (palete.Count == 0)
            {
                Console.WriteLine("Nema upakovanih paleta za slanje.");
                Pause();
                return false;
            }

            Console.WriteLine("\nUpakovane palete spremne za slanje:");
            for (int i = 0; i < palete.Count; i++)
            {
                var p = palete[i];
                Console.WriteLine($"{i + 1}. {p.Sifra} | Serija: {p.Vino.SifraSerije} | {p.Vino.Kategorija} | {p.Vino.KolicinaFlasa} flasa");
            }

            Console.Write("Izaberite paletu (broj): ");
            if (!int.TryParse(Console.ReadLine(), out var izbor) || izbor < 1 || izbor > palete.Count)
            {
                Console.WriteLine("Neispravan izbor.");
                Pause();
                return false;
            }

            var izabrana = palete[izbor - 1];
            var ok = _pakovanje.PosaljiPaletuUSkladiste(_vinskiPodrumId, izabrana.Id);
            Pause();
            return ok;
        }

        private static bool Pause()
        {
            Console.WriteLine("\nPritisnite bilo koji taster za nastavak...");
            Console.ReadKey();
            return true;
        }
    }
}
