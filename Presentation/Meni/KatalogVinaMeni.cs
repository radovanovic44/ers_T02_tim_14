using System;
using System.Linq;
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

        private static string FormatStavka(int rb, string naziv, string kategorija, double zapremina, Guid vinoId, int brojFlasa)
        {
            // BrojFlasa je UKUPNO dostupno (preko vise paleta). Prikazujemo razlaganje po paletama
            // da ne izgleda kao da jedna paleta ima > 24.
            if (brojFlasa <= 24)
            {
                return $"{rb}. {naziv} | {kategorija} | {zapremina} L | ID: {vinoId} | KOLICINA : {brojFlasa}/24";
            }

            int punePalete = brojFlasa / 24;
            int ostatak = brojFlasa % 24;

            string razlaganje = ostatak == 0
                ? $"{punePalete}x24"
                : $"{punePalete}x24 + {ostatak}";

            return $"{rb}. {naziv} | {kategorija} | {zapremina} L | ID: {vinoId} | KOLICINA : {brojFlasa} ({razlaganje})";
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
                Console.WriteLine(FormatStavka(i + 1, k.Naziv, k.Kategorija.ToString(), k.Zapremina, k.VinoId, k.BrojFlasa));
            }

            return true;
        }
    }
}
