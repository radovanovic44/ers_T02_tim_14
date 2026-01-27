using Domain.Enumeracije;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.ProdajaServisi
{
    public class ProdajaServisi : IProdajaServis
    {
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly ISkladistenjeServis _skladisteServis;
        private readonly IFakturaRepozitorijum _fakturaRepo;
        private readonly ILoggerServis _logger;

        public ProdajaServisi(
            IVinoRepozitorijum vinoRepo,
            ISkladistenjeServis skladisteServis,
            IFakturaRepozitorijum fakturaRepo,
            ILoggerServis logger
        )
        {
            _vinoRepo = vinoRepo;
            _skladisteServis = skladisteServis;
            _fakturaRepo = fakturaRepo;
            _logger = logger;
        }

        public IEnumerable<KatalogVinaStavka> VratiKatalogVina()
        {
            var katalog = _vinoRepo.VratiSve()
                .Where(v => _skladisteServis.ImaNaStanju(v.Id))
                .Select(v => new KatalogVinaStavka
                {
                    VinoId = v.Id,
                    Naziv = v.Naziv,
                    Kategorija = v.Kategorija,
                    Zapremina = v.Zapremina
                })
                .OrderBy(x => x.Kategorija)
                .ThenBy(x => x.Naziv)
                .ToList();

            _logger?.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                "Prikazan katalog vina."
            );

            return katalog;
        }

        public Faktura KreirajFakturu(
            Guid vinoId,
            int kolicina,
            TipProdaje tipProdaje,
            NacinPlacanja nacinPlacanja,
            decimal cenaPoFlasi
        )
        {
            if (kolicina <= 0)
                throw new ArgumentException("Količina mora biti > 0.");

            var vino = _vinoRepo.PronadjiPoId(vinoId);
            if (vino == null)
                throw new Exception("Vino ne postoji.");

            var palete = _skladisteServis.IsporuciPalete(1);

            var sviIds = palete.Select(p => p.VinoId).ToList();
            var trazeni = sviIds.Where(id => id == vinoId).Take(kolicina) .ToList();

            if (trazeni.Count < kolicina)
            {
                foreach (var p in palete)
                    _skladisteServis.PrimiPaletu(p);

                throw new Exception("Nema dovoljno vina na stanju.");
            }

            int preostaloZaUkloniti = kolicina;

            foreach (var p in palete)
            {
                if (preostaloZaUkloniti == 0)
                    break;
                if (p.VinoId == vinoId)
                {
                    preostaloZaUkloniti--;
                    
                }
                else
                {
                    _skladisteServis.PrimiPaletu(p);
                }
            }

            var faktura = new Faktura
            {
                Id = Guid.NewGuid(),
                Datum = DateTime.Now,
                TipProdaje = tipProdaje,
                NacinPlacanja = nacinPlacanja,
                Stavke = new List<StavkaFakture>
                {
                    new StavkaFakture
                    {
                        VinoId = vino.Id,
                        NazivVina = vino.Naziv,
                        Kolicina = kolicina,
                        CenaPoFlasi = cenaPoFlasi
                    }
                }
            };

            _fakturaRepo.Dodaj(faktura);

            _logger.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                $"Kreirana faktura {faktura.Id} za vino {vino.Naziv}, količina {kolicina}."
            );

            return faktura;
        }

        public List<Faktura> VratiSveFakture()
        {
            return _fakturaRepo.VratiSve().OrderByDescending(f => f.Datum).ToList();
        }
    }
}
