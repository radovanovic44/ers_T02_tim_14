using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
namespace Services.ProizvodnjaServisi
{
    public class FermentacijaServisi : IProizvodnjaVinaServis
    {
        private readonly IVinovaLozaRepozitorijum _lozaRepo;
        private readonly Queue<Vino> _proizvedena = new();
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly IVinogradarstvoServis _vinogradarstvo;
        private readonly ILoggerServis _logger;

        public FermentacijaServisi(
            IVinovaLozaRepozitorijum lozaRepo,
            IVinoRepozitorijum vinoRepo,
            IVinogradarstvoServis vinogradarstvo,
            ILoggerServis logger)
        {
            _lozaRepo = lozaRepo;
            _vinoRepo = vinoRepo;
            _vinogradarstvo = vinogradarstvo;
            _logger = logger;
        }
        public List<Vino> ZahtevZaVino(KategorijaVina kategorijaVina, int kolicina)
        {
            if (kolicina <= 0) { throw new ArgumentException("Kolicina mora biti veca od nule"); }
            ;
            var rezultat = new List<Vino>();

            while (rezultat.Count < kolicina && _proizvedena.Count > 0)
            {
                var v = _proizvedena.Dequeue();
                if (v.Kategorija == kategorijaVina)
                {
                    rezultat.Add(v);
                }
            }
            if(rezultat.Count < kolicina)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Trazeno {kolicina} vina ({kategorijaVina}), dostupno {rezultat.Count}");
            }



            return rezultat;
        }
    }
}
