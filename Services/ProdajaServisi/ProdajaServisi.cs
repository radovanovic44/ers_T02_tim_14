using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi
{
    public class ProdajaServisi : IProdajaServis
    {
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly ISkladistenjeServis _skladiste;
        private readonly IFakturaRepozitorijum _fakture;
        private readonly ILoggerServis _logger;


        private const int PROCENA_FLASA_PO_PALETI = 24;

        public ProdajaServisi(
            IVinoRepozitorijum vinoRepo,
            ISkladistenjeServis skladiste,
            IFakturaRepozitorijum fakture,
            ILoggerServis logger)
        {
            _vinoRepo = vinoRepo;
            _skladiste = skladiste;
            _fakture = fakture;
            _logger = logger;
        }

        public IEnumerable<KatalogVinaStavka> VratiKatalogVina()
        {
            var katalog = _vinoRepo.VratiSve()
                .Where(v => _skladiste.ImaNaStanju(v.Id))
                .Select(v => new KatalogVinaStavka
                {
                    VinoId = v.Id,
                    Naziv = v.Naziv,
                    Kategorija = v.Kategorija,
                    Zapremina = v.Zapremina,
                    BrojFlasa = _skladiste.DostupnoFlasa(v.Id)
                })
                .OrderBy(x => x.Kategorija)
                .ThenBy(x => x.Naziv)
                .ToList();

            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, "Prikazan katalog vina.");
            return katalog;
        }

        public Faktura KreirajFakturu(
            Guid vinoId,
            int kolicina,
            TipProdaje tipProdaje,
            NacinPlacanja nacinPlacanja,
            decimal cenaPoFlasi)
        {
            if (kolicina <= 0) throw new ArgumentException("Količina mora biti > 0.");

            var vino = _vinoRepo.PronadjiPoId(vinoId);
            if (vino == null) throw new Exception("Vino ne postoji.");


            int trazenePalete = (int)Math.Ceiling((double)kolicina / PROCENA_FLASA_PO_PALETI);
            var palete = _skladiste.IsporuciPaleteZaVino(vinoId, trazenePalete);


            int dostupno = _skladiste.DostupnoFlasa(vinoId);
            if (dostupno < kolicina)
                throw new Exception($"Nema dovoljno vina na stanju. Trazeno: {kolicina}, dostupno: {dostupno}.");

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

            _fakture.Dodaj(faktura);


            _vinoRepo.Obrisi(vinoId);

            _logger.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                $"Kreirana faktura {faktura.Id} za vino {vino.Naziv}, kolicina {kolicina}.");

            return faktura;
        }

        public List<Faktura> VratiSveFakture()
        {
            return _fakture.VratiSve().OrderByDescending(f => f.Datum).ToList();
        }
    }
}