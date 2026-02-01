using Domain.Servisi;

namespace Presentation.Meni
{
    public class KatalogVinaMeni
    {
        private readonly IProdajaServis _prodaja;

        public KatalogVinaMeni(IProdajaServis prodaja)
        {
            _prodaja = prodaja;
        }

        public bool Prikazi()
        {
            var katalog = _prodaja.VratiKatalogVina().ToList();

            Console.WriteLine("=== KATALOG VINA ===");
            if (!katalog.Any())
            {
                Console.WriteLine("Nema dostupnih vina.");
                return false;
            }

            for (int i = 0; i < katalog.Count; i++)
            {
                var k = katalog[i];
                Console.WriteLine($"{i + 1}. {k.Naziv} | {k.Kategorija} | {k.Zapremina} L | ID: {k.VinoId} | KOLICINA : {k.BrojFlasa}/24");
            }

            return true;
        }
    }
}
