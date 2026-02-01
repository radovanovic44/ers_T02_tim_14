using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly Korisnik _korisnik;
        private readonly IProizvodnjaVinaServis _proizvodnja;
        private readonly IPakovanjeServis _pakovanje;
        private readonly KatalogVinaMeni _katalogMeni;
        private readonly FaktureMeni _faktureMeni;

        // Pretpostavimo jedan (glavni) vinski podrum za demo
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
                Console.WriteLine("================================ MENI ================================");
                Console.WriteLine($"Prijavljeni korisnik: {_korisnik.ImePrezime} ({_korisnik.Uloga})");
                Console.WriteLine();

                if (_korisnik.Uloga == TipKorisnika.GLAVNI_ENOLOG)
                {
                    Console.WriteLine("1. Pokreni fermentaciju");
                    Console.WriteLine("2. Upakuj vina u paletu");
                    Console.WriteLine("3. Posalji paletu u skladiste");
                    Console.WriteLine("4. Pregled svih faktura");
                    Console.WriteLine("5. Katalog vina");
                }
                else // KELAR_MAJSTOR
                {
                    Console.WriteLine("1. Katalog vina");
                    Console.WriteLine("2. Prodaja (kreiranje fakture)");
                    Console.WriteLine("3. Pregled svih faktura");
                }

                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");
                var izbor = Console.ReadLine() ?? "";

                if (izbor == "0") return true;

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
                            _pakovanje.PosaljiPaletuUSkladiste(_vinskiPodrumId);
                            Pause();
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

            Console.Write("Broj flasa za pakovanje: ");
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

            var (ok, paleta) = _pakovanje.UpakujVina(_vinskiPodrumId, kategorija, broj, zap);
            Console.WriteLine(ok ? $"Upakovana paleta: {paleta.Sifra} ({paleta.Vino.KolicinaFlasa} flasa)" : "Pakovanje nije uspelo.");
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
