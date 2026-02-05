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

        public (bool, Paleta) UpakujVina(Guid vinskiPodrumId, KategorijaVina kategorija, int brojFlasa, double zapreminaFlase)
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

                
                var vino = _proizvodnjaVina.ZahtevZaVino(
                     _vinoRepo.VratiSve()
                    .Where(v => v.Kategorija == kategorija)
                    .Where(v => !_paleteRepo.SvePalete().Any(p => p.Vino != null && p.Vino.Id == v.Id))
                    .Select(v => v.Id)
                    .FirstOrDefault(),
                    brojFlasa);
                var paleta = new Paleta();
               
                if (vino.KolicinaFlasa < brojFlasa)
                {
                    var okFer = _proizvodnjaVina.ZapocniFermentaciju(kategorija, brojFlasa - vino.KolicinaFlasa, zapreminaFlase);
                    if (!okFer)
                        return (false, new Paleta());

                    var dopuna = _proizvodnjaVina.ZahtevZaVino(
                     _vinoRepo.VratiSve()
                    .Where(v => v.Kategorija == kategorija)
                    .Where(v => !_paleteRepo.SvePalete().Any(p => p.Vino != null && p.Vino.Id == v.Id))
                    .Select(v => v.Id)
                    .FirstOrDefault(),
                    brojFlasa);

                    paleta.Sifra = $"PL-{DateTime.Now:yyyy}-{paleta.Id}";
                    paleta.AdresaOdredista = string.Empty;
                    paleta.VinskiPodrumId = vinskiPodrumId;
                    paleta.Vino = dopuna;
                    paleta.Status = StatusPalete.Upakovana;

                }
                else
                {
                    paleta.Sifra = $"PL-{DateTime.Now:yyyy}-{paleta.Id}";
                    paleta.AdresaOdredista = string.Empty;
                    paleta.VinskiPodrumId = vinskiPodrumId;
                    paleta.Vino = vino;
                    paleta.Status = StatusPalete.Upakovana;
                }

                _paleteRepo.DodajPaletu(paleta);

                _logger.EvidentirajDogadjaj(
                    TipEvidencije.INFO,
                    $"Upakovana paleta {paleta.Sifra} ({paleta.Vino.KolicinaFlasa} flasa) - kategorija {kategorija}.");

                return (true, paleta);
            }
            catch (Exception ex)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska pakovanja: {ex.Message}");
                return (false, new Paleta());
            }
        }

        public bool PosaljiPaletuUSkladiste(Guid vinskiPodrumId)
        {
            try
            {
                var paleta = _paleteRepo.SvePalete()
                    .FirstOrDefault(p => p.VinskiPodrumId == vinskiPodrumId && p.Status == StatusPalete.Upakovana);

                if (paleta == null)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, "Nema dostupne upakovane palete za slanje.");
                    return false;
                }

                paleta.Status = StatusPalete.Otpremljena;

                var ok = _skladistenje.PrimiPaletu(paleta);
                _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Paleta '{paleta.Sifra}' poslata u skladiste.");
                return ok;
            }
            catch (Exception ex)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska slanja palete: {ex.Message}");
                return false;
            }
        }
    }
}
