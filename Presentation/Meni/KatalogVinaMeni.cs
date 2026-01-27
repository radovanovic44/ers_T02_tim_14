using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class KatalogVinaMeni
    {
        private readonly IProdajaServis _prodajaServis;

        public KatalogVinaMeni(IProdajaServis prodajaServis)
        {
            _prodajaServis = prodajaServis;
        }

        public void Prikazi()
        {
            var katalog = _prodajaServis.VratiKatalogVina().ToList();

            Console.WriteLine("=== KATALOG VINA ===");
            if (!katalog.Any())
            {
                Console.WriteLine("Nema dostupnih vina.");
                return;
            }

            for (int i = 0; i < katalog.Count; i++)
            {
                var k = katalog[i];
                Console.WriteLine($"{i + 1}. {k.Naziv} | {k.Kategorija} | {k.Zapremina} L");
            }
        }
    }
}
