using Domain.Enumeracije;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class FaktureMeni
    {
        private readonly IProdajaServis _prodaja;

        public FaktureMeni(IProdajaServis prodaja)
        {
            _prodaja = prodaja;
        }

        public bool KreirajFakturu()
        {
            Console.Write("Unesi ID vina: ");
            if (!Guid.TryParse(Console.ReadLine(), out var vinoId))
            {
                Console.WriteLine("Neispravan ID.");
                return false;
            }

            Console.Write("Količina: ");
            if (!int.TryParse(Console.ReadLine(), out var kolicina) || kolicina <= 0)
            {
                Console.WriteLine("Neispravna količina.");
                return false;
            }

            Console.Write("Cena po flasi: ");
            if (!decimal.TryParse(Console.ReadLine(), out var cena) || cena <= 0)
            {
                Console.WriteLine("Neispravna cena.");
                return false;
            }

            Console.WriteLine("Tip prodaje: 1-RestoranskaProdaja, 2-DiskontPica");
            var tip = Console.ReadLine() == "2" ? TipProdaje.DiskontPica : TipProdaje.RestoranskaProdaja;

            Console.WriteLine("Način plaćanja: 1-Gotovina, 2-Predracun, 3-GotovinskiRacun");
            var np = Console.ReadLine();
            var nacin = np switch
            {
                "2" => NacinPlacanja.Predracun,
                "3" => NacinPlacanja.GotovinskiRacun,
                _ => NacinPlacanja.Gotovina
            };

            try
            {
                var faktura = _prodaja.KreirajFakturu(vinoId, kolicina, tip, nacin, cena);
                Console.WriteLine($"Faktura kreirana: {faktura.Id}");
                Console.WriteLine($"Ukupan iznos: {faktura.UkupanIznos}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool PregledSvihFaktura()
        {
            var fakture = _prodaja.VratiSveFakture();

            Console.WriteLine("=== SVE FAKTURE ===");
            if (!fakture.Any())
            {
                Console.WriteLine("Nema faktura.");
                return false;
            }

            foreach (var f in fakture)
            {
                Console.WriteLine($"Faktura: {f.Id} | {f.Datum:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Tip: {f.TipProdaje} | Plaćanje: {f.NacinPlacanja}");
                foreach (var s in f.Stavke)
                    Console.WriteLine($"- {s.NazivVina} x{s.Kolicina} = {s.UkupnaCena}");
                Console.WriteLine($"UKUPNO: {f.UkupanIznos}");
            }

            return true;
        }
    }
}
