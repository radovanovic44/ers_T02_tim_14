using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.PakovanjeServisi
{
    public class PakovanjeServis : IPakovanjeServis
    {
        private readonly IPaleteRepozitorijum _paleteRepo;
        private readonly IProizvodnjaVinaServis _proizvodnjaVina;
        private readonly ISkladistenjeServis _skladistenje;
        private readonly ILoggerServis _logger;
        private readonly IVinoRepozitorijum _vinoRepo;

        private const int MAX_FLASA_PO_PALETI = 24;

        public PakovanjeServis(
            IPaleteRepozitorijum paleteRepo,
            IVinoRepozitorijum vinoRepo,
            IProizvodnjaVinaServis proizvodnjaVina,
            ISkladistenjeServis skladistenje,
            ILoggerServis logger)
        {
            _paleteRepo = paleteRepo;
            _vinoRepo = vinoRepo;
            _proizvodnjaVina = proizvodnjaVina;
            _skladistenje = skladistenje;
            _logger = logger;
        }

        public IEnumerable<Vino> VratiVinaZaPakovanje(KategorijaVina kategorija)
        {
            try
            {
                return _vinoRepo.VratiSve()
                    .Where(v => v.Kategorija == kategorija)
                    .Where(v => v.KolicinaFlasa > 0)
                    .ToList();
            }
            catch
            {
                return [];
            }
        }

        public (bool, Paleta) UpakujVina(Guid vinskiPodrumId, Guid vinoId, int brojFlasa, double zapreminaFlase)
        {
            try
            {
                if (brojFlasa <= 0)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, "Pokusaj pakovanja sa nevalidnom kolicinom.");
                    return (false, new Paleta());
                }

                if (brojFlasa > MAX_FLASA_PO_PALETI)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Ogranicenje: max {MAX_FLASA_PO_PALETI} flasa po paleti.");
                    brojFlasa = MAX_FLASA_PO_PALETI;
                }

                var izabrano = _vinoRepo.PronadjiPoId(vinoId);
                if (izabrano == null)
                {
                    Console.WriteLine("Izabrano vino ne postoji.");
                    return (false, new Paleta());
                }

                Vino vino;
                try
                {
                    vino = _proizvodnjaVina.ZahtevZaVino(vinoId, brojFlasa);
                }
                catch
                {
                    // Ako nema dovoljno na stanju, pokusaj automatski da dopunis fermentacijom
                    var manjka = Math.Max(0, brojFlasa - izabrano.KolicinaFlasa);
                    if (manjka > 0)
                    {
                        var okFer = _proizvodnjaVina.ZapocniFermentaciju(izabrano.Kategorija, manjka, zapreminaFlase);
                        if (!okFer)
                            return (false, new Paleta());
                    }

                    // Nakon fermentacije, uzmi bilo koje vino iste kategorije koje moze da pokrije kolicinu (najbolje dostupno)
                    var kandidatId = _vinoRepo.VratiSve()
                        .Where(v => v.Kategorija == izabrano.Kategorija)
                        .OrderByDescending(v => v.KolicinaFlasa)
                        .Select(v => v.Id)
                        .FirstOrDefault();

                    vino = _proizvodnjaVina.ZahtevZaVino(kandidatId, brojFlasa);
                }

                var paleta = new Paleta
                {
                    Sifra = $"PL-{DateTime.Now:yyyy}-{Guid.NewGuid()}",
                    AdresaOdredista = string.Empty,
                    VinskiPodrumId = vinskiPodrumId,
                    Vino = vino,
                    Status = StatusPalete.Upakovana
                };

                _paleteRepo.DodajPaletu(paleta);

                _logger.EvidentirajDogadjaj(
                    TipEvidencije.INFO,
                    $"Upakovana paleta {paleta.Sifra} ({paleta.Vino.KolicinaFlasa} flasa) - kategorija {paleta.Vino.Kategorija}.");

                return (true, paleta);
            }
            catch (Exception ex)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska pakovanja: {ex.Message}");
                return (false, new Paleta());
            }
        }

     
        public (bool, Paleta) UpakujVina(Guid vinskiPodrumId, KategorijaVina kategorija, int brojFlasa, double zapreminaFlase)
        {
            var vinoId = _vinoRepo.VratiSve()
                .Where(v => v.Kategorija == kategorija)
                .OrderByDescending(v => v.KolicinaFlasa)
                .Select(v => v.Id)
                .FirstOrDefault();

            return UpakujVina(vinskiPodrumId, vinoId, brojFlasa, zapreminaFlase);
        }

        public IEnumerable<Paleta> VratiUpakovanePalete(Guid vinskiPodrumId)
        {
            try
            {
                return _paleteRepo.SvePalete()
                    .Where(p => p.VinskiPodrumId == vinskiPodrumId && p.Status == StatusPalete.Upakovana)
                    .ToList();
            }
            catch
            {
                return [];
            }
        }

        public bool PosaljiPaletuUSkladiste(Guid vinskiPodrumId, Guid paletaId)
        {
            try
            {
                var paleta = _paleteRepo.SvePalete()
                    .FirstOrDefault(p => p.VinskiPodrumId == vinskiPodrumId && p.Id == paletaId && p.Status == StatusPalete.Upakovana);

                if (paleta == null)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, "Nema izabrane/upakovane palete za slanje.");
                    Console.WriteLine("Nema dostupne upakovane palete za slanje.");
                    return false;
                }

                paleta.Status = StatusPalete.Otpremljena;

                var ok = _skladistenje.PrimiPaletu(paleta);
                _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Paleta '{paleta.Sifra}' poslata u skladiste.");
                Console.WriteLine($"Paleta '{paleta.Sifra}' poslata u skladiste.");
                return ok;
            }
            catch (Exception ex)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska slanja palete: {ex.Message}");
                return false;
            }
        }

        
        public bool PosaljiPaletuUSkladiste(Guid vinskiPodrumId)
        {
            var prva = VratiUpakovanePalete(vinskiPodrumId).FirstOrDefault();
            if (prva == null) return false;
            return PosaljiPaletuUSkladiste(vinskiPodrumId, prva.Id);
        }
    }
}
